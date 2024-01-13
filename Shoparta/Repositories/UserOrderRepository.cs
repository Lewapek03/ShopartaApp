using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoparta.Data;
using Shoparta.Models;

namespace Shoparta.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var user = GetCurrentUser();

            // Sprawdzamy, czy użytkownik to administrator
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                // Jeżeli tak, zwracamy wszystkie zamówienia
                return await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                                .ThenInclude(x => x.Item)
                                .ThenInclude(x => x.Category)
                            .ToListAsync();
            }
            else
            {
                // Jeżeli nie, zwracamy tylko zamówienia dla zalogowanego użytkownika
                return await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                                .ThenInclude(x => x.Item)
                                .ThenInclude(x => x.Category)
                            .Where(a => a.UserId == user.Id)
                            .ToListAsync();
            }
        }

        private IdentityUser GetCurrentUser()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            return _userManager.FindByIdAsync(userId).Result;
        }
    }
}