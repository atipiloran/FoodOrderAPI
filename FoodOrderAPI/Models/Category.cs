using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderAPI.Models
{
    public class Category
    {
        [Key] // Specifies that the following property is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies that the database will generate the value for this property
        public int Id { get; set; } // Represents the unique identifier for the category

        [Required] // Specifies that the following property is required (not nullable)
        [MaxLength(255)] // Specifies the maximum length of the string
        public string Name { get; set; } // Represents the name of the category

        public string Description { get; set; } // Represents the description of the category
    }
}
