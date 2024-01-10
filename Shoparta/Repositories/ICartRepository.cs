using Shoparta.Models;

namespace Shoparta.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int itemId, int qty);
        Task<int> RemoveItem(int itemId);
        Task<ShoppingCart> UserCart();
        public Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> Checkout();
    }
}
