using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Order;
using RuyaOptik.Entity.Concrete;
using RuyaOptik.Entity.Enums;

namespace RuyaOptik.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly RuyaOptikDbContext _db;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(
            RuyaOptikDbContext db,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            ICartRepository cartRepository,
            IMapper mapper)
        {
            _db = db;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }


        // CREATE ORDER (TRANSACTION)

        public async Task<OrderDto> CreateAsync(OrderCreateDto dto)
        {
            if (!dto.Items.Any())
                throw new Exception("Order must contain at least one item.");

            await using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = dto.UserId,
                    CustomerName = dto.CustomerName,
                    PhoneNumber = dto.PhoneNumber,
                    AddressLine = dto.AddressLine,
                    City = dto.City,
                    District = dto.District,
                    PostalCode = dto.PostalCode,
                    Status = OrderStatus.Pending,
                    TotalPrice = 0
                };

                foreach (var item in dto.Items)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null || product.IsDeleted || !product.IsActive)
                        throw new Exception($"Product not found: {item.ProductId}");

                    var inventory = await _inventoryRepository.GetByProductIdAsync(item.ProductId);
                    if (inventory == null)
                        throw new Exception($"Inventory not found for product: {item.ProductId}");

                    var available = inventory.Quantity - inventory.Reserved;
                    if (available < item.Quantity)
                        throw new Exception($"Insufficient stock for product: {product.Name}");

                    var unitPrice = product.DiscountedPrice ?? product.Price;

                    // reserve stock
                    inventory.Reserved += item.Quantity;
                    inventory.UpdatedDate = DateTime.UtcNow;
                    await _inventoryRepository.UpdateAsync(inventory);

                    order.Items.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice
                    });

                    order.TotalPrice += unitPrice * item.Quantity;
                }

                await _orderRepository.AddAsync(order);
                await _orderRepository.SaveChangesAsync();

                // clear cart after successful order
                var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);
                if (cart != null)
                {
                    foreach (var ci in cart.Items.ToList())
                        await _cartRepository.DeleteCartItemAsync(ci);

                    cart.TotalPrice = 0;
                    cart.UpdatedDate = DateTime.UtcNow;
                    await _cartRepository.SaveChangesAsync();
                }

                await tx.CommitAsync();
                return _mapper.Map<OrderDto>(order);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }


        // UPDATE ORDER STATUS

        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus status)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                var order = await _orderRepository.GetOrderWithItemsAsync(orderId);
                if (order == null || order.IsDeleted)
                    return false;

                // same status -> no-op
                if (order.Status == status)
                    return true;

                // finalized guard
                if (order.Status == OrderStatus.Delivered ||
                    order.Status == OrderStatus.Cancelled)
                    throw new Exception("Finalized orders cannot be updated.");

                // DELIVERED → stock düş
                if (status == OrderStatus.Delivered)
                {
                    foreach (var item in order.Items)
                    {
                        var inventory = await _inventoryRepository.GetByProductIdAsync(item.ProductId);
                        if (inventory == null)
                            throw new Exception($"Inventory not found for product: {item.ProductId}");

                        inventory.Reserved = Math.Max(0, inventory.Reserved - item.Quantity);
                        inventory.Quantity -= item.Quantity;

                        if (inventory.Quantity < 0)
                            throw new Exception("Inventory quantity cannot be negative.");

                        inventory.UpdatedDate = DateTime.UtcNow;
                        await _inventoryRepository.UpdateAsync(inventory);
                    }
                }

                // CANCELLED → reserve geri bırak
                if (status == OrderStatus.Cancelled)
                {
                    foreach (var item in order.Items)
                    {
                        var inventory = await _inventoryRepository.GetByProductIdAsync(item.ProductId);
                        if (inventory == null)
                            throw new Exception($"Inventory not found for product: {item.ProductId}");

                        inventory.Reserved = Math.Max(0, inventory.Reserved - item.Quantity);
                        inventory.UpdatedDate = DateTime.UtcNow;
                        await _inventoryRepository.UpdateAsync(inventory);
                    }
                }

                order.Status = status;
                order.UpdatedDate = DateTime.UtcNow;

                await _orderRepository.UpdateAsync(order);
                await _orderRepository.SaveChangesAsync();

                await tx.CommitAsync();
                return true;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }


        // GET USER ORDERS

        public async Task<List<OrderDto>> GetByUserIdAsync(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
