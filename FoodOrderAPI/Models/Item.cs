using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderAPI.Models
{
    public class Item
    {
        [Key] // Specifies that the following property is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies that the database will generate the value for this property
        public int Id { get; set; } // Represents the unique identifier for the item

        [Required] // Specifies that the following property is required (not nullable)
        [MaxLength(255)] // Specifies the maximum length of the string
        public string Name { get; set; } // Represents the name of the item

        public string Description { get; set; } // Represents the description of the item

        [Required] // Specifies that the following property is required (not nullable)
        public double Price { get; set; } // Represents the price of the item

        [ForeignKey("Category")] // Specifies the foreign key relationship with the Category entity
        [Required] // Specifies that the following property is required (not nullable)
        public int CategoryId { get; set; } // Represents the foreign key reference to the Category entity

        public Category Category { get; set; } // Represents the navigation property for the related Category entity
    }
}
