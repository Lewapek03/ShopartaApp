using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoparta.Models;
using Shoparta.Models.DTOs;
using Shoparta.Repositories;
using System.Diagnostics;


namespace Shoparta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _homeRepository.CreateCategory(new Category { Name = category.Name });
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        

        [Authorize(Roles = "Admin")]
        [HttpPost]        
        public async Task<IActionResult> CreateItem(CreateEditItemDTO creatEditItemDto)
        {
            if (ModelState.IsValid)
            {
                var newItem = new Item
                {
                    Name = creatEditItemDto.Name,
                    Image = creatEditItemDto.Image,
                    Description = creatEditItemDto.Description,
                    Price = creatEditItemDto.Price,
                    CategoryId = creatEditItemDto.CategoryId
                };

                await _homeRepository.CreateItem(newItem);
                return RedirectToAction("Index");
            }
                       

            return View(creatEditItemDto);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveItem([FromBody] int id)
        {            
            bool result = await _homeRepository.RemoveItem(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                // W przypadku błędu można przekierować do strony błędu lub zwrócić komunikat.
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")] // Asumuję, że tylko admin może dodawać produkty
        public async Task<IActionResult> CreateItem()
        {
            ViewBag.Categories = new SelectList(await _homeRepository.Categories(), "Id", "Name");
            return View();
        }


        public async Task <IActionResult> Index(string searchItem ="", int categoryId =0)
        {
            
            IEnumerable<Item> items =await _homeRepository.GetItems(searchItem, categoryId);
            IEnumerable<Category> categories = await _homeRepository.Categories();
            ItemDTO itemDTO = new ItemDTO {
                Items = items,
                Categories = categories,
                SearchItem = searchItem,
                CategoryId = categoryId
            };
            return View(itemDTO);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        

    }
}