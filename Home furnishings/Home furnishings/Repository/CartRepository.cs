// Repository/CartRepository.cs
using Home_furnishings.Models;
using Microsoft.EntityFrameworkCore;

namespace Home_furnishings.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly Context _context;

        public CartRepository(Context context)
        {
            _context = context;
        }

        public List<Cart> GetAll()
        {
            return _context.Carts
                .Include(c => c.Products)
                .Include(c => c.User)
                .ToList();
        }

        public Cart GetById(int id)
        {
            return _context.Carts
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CartId == id);
        }

        public void Insert(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void Update(int id, Cart cart)
        {
            var entity = _context.Carts.Find(id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(cart);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var entity = _context.Carts
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CartId == id);

            if (entity != null)
            {
                _context.Carts.Remove(entity);
                _context.SaveChanges();
            }
        }

        public Cart GetCartByUserId(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.Products)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Products = new List<Product>()
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            return cart;
        }

        public Cart GetCartWithProducts(int userId)
        {
            return _context.Carts
                .Include(c => c.Products)
                    .ThenInclude(p => p.Category)
                .FirstOrDefault(c => c.UserId == userId);
        }

        public void AddProductToCart(int cartId, int productId)
        {
            var cart = _context.Carts
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CartId == cartId);

            var product = _context.Products.Find(productId);

            if (cart != null && product != null && !cart.Products.Any(p => p.ProductId == productId))
            {
                cart.Products.Add(product);
                _context.SaveChanges();
            }
        }

        public void RemoveProductFromCart(int cartId, int productId)
        {
            var cart = _context.Carts
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CartId == cartId);

            if (cart != null)
            {
                var product = cart.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    cart.Products.Remove(product);
                    _context.SaveChanges();
                }
            }
        }

        public void ClearCart(int userId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                cart.Products.Clear();
                _context.SaveChanges();
            }
        }

        public int GetCartItemCount(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.Products)
                .FirstOrDefault(c => c.UserId == userId);

            return cart?.Products.Count ?? 0;
        }
    }
}