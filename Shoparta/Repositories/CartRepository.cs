using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shoparta.Data;
using Shoparta.Models;
using Microsoft.IdentityModel.Tokens;

namespace Shoparta.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager =  userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            var card = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return card;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId=GetUserId();
            }
            var CardItems = await (from cart in _db.ShoppingCarts
                                  join cardDetail in _db.CardDetail
                                  on cart.Id equals cardDetail.ShoppingCartId
                                   where cart.UserId == userId
                                   select new { cardDetail.Id }).ToListAsync();
            return CardItems.Count;
        }

        public async Task<int> AddItem(int itemId, int quantity)
        {
            using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {                                
                if (string.IsNullOrEmpty(userId)) throw new Exception("User not logged"); 
                var card = await GetCart(userId);
                if (card == null)
                {
                    card = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(card);
                    _db.SaveChanges();

                }
                var cartItem = _db.CardDetail.FirstOrDefault(x => x.ShoppingCartId == card.Id && x.ItemId == itemId);
                if (cartItem is not null)
                {

                    cartItem.Quantity += quantity;
                }
                else
                {
                    var item = _db.Items.Find(itemId);
                    cartItem = new CardDetail
                    {
                        ItemId = itemId,
                        ShoppingCartId = card.Id,
                        Quantity = quantity,
                        UnitPrice = item.Price,                        
                    };
                    _db.CardDetail.Add(cartItem);

                }
                await _db.SaveChangesAsync();
                transaction.Commit();               
            }
            catch (Exception ex) { }
            return await GetCartItemCount(userId);

        }
        public async Task<int> RemoveItem(int itemId)
        {
            string userId = GetUserId();
            try
            {                               
                if (string.IsNullOrEmpty(userId)) throw new Exception("User not logged");
                var card = await GetCart(userId);
                if (card == null)
                {
                    throw new Exception("Invalid cart");
                }
                var cartItem = _db.CardDetail.FirstOrDefault(x => x.ShoppingCartId == card.Id && x.ItemId == itemId);
                if (cartItem is null)
                {
                    throw new Exception("Not items in your cart");
                }
                else if (cartItem.Quantity==1)
                {
                    _db.CardDetail.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= 1;
                }
                _db.SaveChanges();
                
            }
            catch (Exception ex) {}
            return await GetCartItemCount(userId);
        }
        private string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            return userId;

        }
        public async Task<ShoppingCart> UserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid user id");
            var Cart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Item)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return Cart;

        }
        public async Task<bool> Checkout()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Your card is invalid");
                var cartDetail = _db.CardDetail
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    OrderStatusId = 1//New
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ItemId = item.ItemId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();

                // removing the cartdetails
                _db.CardDetail.RemoveRange(cartDetail);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
