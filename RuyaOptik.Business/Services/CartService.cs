using AutoMapper;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Cart;
using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.Business.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, TotalPrice = 0 };
                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            RecalculateCartTotal(cart);
            await _cartRepository.SaveChangesAsync();

            return MapCart(cart);
        }

        public async Task<CartDto> AddToCartAsync(AddToCartDto dto)
        {
            if (dto.Quantity <= 0) dto.Quantity = 1;

            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null || product.IsDeleted || !product.IsActive)
                throw new Exception($"Product not found: {dto.ProductId}");

            var cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId);
            if (cart == null)
            {
                cart = new Cart { UserId = dto.UserId, TotalPrice = 0 };
                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            var unitPrice = product.DiscountedPrice ?? product.Price;

            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                existingItem.UnitPrice = unitPrice;
                existingItem.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = unitPrice
                };

                await _cartRepository.AddCartItemAsync(newItem);
            }

            // total güncelle
            // cart.Items Include ile geliyor; ama yeni item eklendiyse listede olmayabilir.
            // En sağlamı: tekrar cart'ı include ile çekmek.
            await _cartRepository.SaveChangesAsync();

            cart = await _cartRepository.GetCartByUserIdAsync(dto.UserId) ?? cart;
            RecalculateCartTotal(cart);
            await _cartRepository.SaveChangesAsync();

            return MapCart(cart);
        }

        public async Task<bool> RemoveItemAsync(int cartItemId)
        {
            var item = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (item == null) return false;

            // cart'ı al
            var cart = await _cartRepository.GetByIdAsync(item.CartId);
            if (cart == null || cart.IsDeleted) return false;

            await _cartRepository.DeleteCartItemAsync(item);
            await _cartRepository.SaveChangesAsync();

            // cart total recalculation
            var fullCart = await _cartRepository.GetCartByUserIdAsync(cart.UserId);
            if (fullCart != null)
            {
                RecalculateCartTotal(fullCart);
                await _cartRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            // tüm item’ları sil
            foreach (var item in cart.Items.ToList())
            {
                await _cartRepository.DeleteCartItemAsync(item);
            }

            cart.TotalPrice = 0;
            cart.UpdatedDate = DateTime.UtcNow;

            await _cartRepository.SaveChangesAsync();
            return true;
        }

        private void RecalculateCartTotal(Cart cart)
        {
            // Items boşsa 0
            cart.TotalPrice = cart.Items?.Where(i => !i.IsDeleted).Sum(i => i.UnitPrice * i.Quantity) ?? 0;
            cart.UpdatedDate = DateTime.UtcNow;
        }

        private CartDto MapCart(Cart cart)
        {
            var dto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
                Items = cart.Items
                    .Where(i => !i.IsDeleted)
                    .Select(i => new CartItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product?.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.UnitPrice * i.Quantity
                    })
                    .ToList()
            };

            return dto;
        }
    }
}
