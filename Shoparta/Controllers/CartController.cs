using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoparta.Repositories;
using System.Net;

namespace Shoparta.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cardRepository;
        public CartController(ICartRepository cardRepository)
        {
            _cardRepository = cardRepository;     
        }
        public async Task<IActionResult> AddItem(int itemId, int quantity=1, int redirect=0)
        {
            var cartCount = await _cardRepository.AddItem(itemId, quantity);
            if (redirect == 0) {
                return Ok(cartCount);                
            }
            return RedirectToAction("UserCart");
        }
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var cartCount = await _cardRepository.RemoveItem(itemId);
            return RedirectToAction("UserCart");
        }
        public async Task<IActionResult> GetCartItemCount()
        {
            int cartItemCount = await _cardRepository.GetCartItemCount();
            return Ok(cartItemCount);
        }
        public async Task<IActionResult> UserCart()
        {
            var cart = await _cardRepository.UserCart();
            return View(cart);
        }
        public async Task<IActionResult> Checkout()
        {
            bool isCheckedOut = await _cardRepository.Checkout();
            if (!isCheckedOut)
                throw new Exception("Something happen in server side");
            return RedirectToAction("Index", "Home");
        }
    }
}
