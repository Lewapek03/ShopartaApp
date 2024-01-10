using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoparta.Models
{
    [Table("CardDetail")]
    public class CardDetail
    {
        public int Id { get; set; }

        public Item Item { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int Quantity {  get; set; }

        public int UnitPrice { get; set; }



    }
}
