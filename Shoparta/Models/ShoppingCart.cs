using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoparta.Models
{
    [Table("ShoppingCart")]

    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<CardDetail> CartDetails { get; set; }
    }
}
