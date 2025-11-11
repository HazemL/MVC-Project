using Home_furnishings.Models;

namespace Home_furnishings.Repository
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetCartByUserId(int userId);
        Cart GetCartWithProducts(int userId);
        void AddProductToCart(int cartId, int productId);
        void RemoveProductFromCart(int cartId, int productId);
        void ClearCart(int userId);
        int GetCartItemCount(int userId);
    }
}