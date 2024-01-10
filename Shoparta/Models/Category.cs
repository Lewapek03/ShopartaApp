using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shoparta.Models
{    

    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }

        [Required,MaxLength(80)]
        public string Name { get; set; }
        
        public List<Item>? Items { get; set; }



    }
}
