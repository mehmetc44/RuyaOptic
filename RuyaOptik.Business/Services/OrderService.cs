using AutoMapper;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Order;
using RuyaOptik.Entity.Entities.Concrete;
using RuyaOptik.Entity.Entities.Enums;

namespace RuyaOptik.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateAsync(OrderCreateDto dto)
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

                if (inventory.Quantity - inventory.Reserved < item.Quantity)
                    throw new Exception($"Insufficient stock for product: {product.Name}");

                var unitPrice = product.DiscountedPrice ?? product.Price;

                inventory.Reserved += item.Quantity;
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

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetByUserIdAsync(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
