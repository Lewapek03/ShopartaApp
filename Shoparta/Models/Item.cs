using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoparta.Models
{
    [Table("Item")]
    public class Item
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string? Name { get; set; }

        [Required, MaxLength(150)]
        public string? Description { get; set; }

        [Required, Range(1, 100000000, ErrorMessage = "Price should between from 1 to 100 000 000.")]
        public int Price { get; set; } = 1;

        public string? Image { get; set; }

        public int CategoryId { get; set; }
       
        public Category Category { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }

        [NotMapped] public String CategoryName { get; set;}

    }
}
