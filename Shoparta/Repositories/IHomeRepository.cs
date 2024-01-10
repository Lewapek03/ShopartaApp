using Shoparta.Models;
using System.Threading.Tasks;
using Shoparta.Models.DTOs;

namespace Shoparta.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Item>> GetItems(string seachItem = "", int categoryId = 0);
        Task<IEnumerable<Category>> Categories();
        Task CreateCategory(Category category);
        Task  CreateItem(Item item);
        Task<Item> GetItemById(int id);
        Task<bool> RemoveItem(int id);
    }
}