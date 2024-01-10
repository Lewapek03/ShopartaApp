namespace Shoparta.Models.DTOs
{
    public class ItemDTO
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string SearchItem { get; set; } = string.Empty;
        public int CategoryId { get; set; } = 0;

    }
}
