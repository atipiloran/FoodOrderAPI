using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderAPI.Models.Dto
{
    public class AddItemDTO
    {
        // Represents the name of the item
        public string Name { get; set; }

        // Represents the description of the item
        public string Description { get; set; }

        // Represents the price of the item
        public double Price { get; set; }

        // Represents the ID of the category to which the item belongs
        public int CategoryId { get; set; }
    }
}
