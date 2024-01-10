using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Shoparta.Models;
public class BulkEditViewModel
        {
            public List<CreateEditItemDTO> Items { get; set; }
        }

        public class CreateEditItemDTO
    {
            public int Id { get; set; }

            [Required(ErrorMessage = "Item name is required."), MaxLength(50)]
            public string Name { get; set; } 

            [Required(ErrorMessage = "Description is required."), MaxLength(150)]
            public string Description { get; set; }

            [Required(ErrorMessage = "Price is required."), Range(1, 100000000)]
            public int Price { get; set; }

            // Assuming image URL is optional; if it's required, add [Required]
            public string Image { get; set; }

            [Required(ErrorMessage = "Category is required.")]
            public int CategoryId { get; set; }
        }
