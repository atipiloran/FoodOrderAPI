using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderAPI.Models
{
    public class Address
    {
        [Key] // Specifies that the following property is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies that the database will generate the value for this property
        public int Id { get; set; } // Represents the unique identifier for the address

        [Required] // Specifies that the following property is required (not nullable)
        [MaxLength(255)] // Specifies the maximum length of the string
        public string Name { get; set; } // Represents the name of the address

        [Required] // Specifies that the following property is required (not nullable)
        [MaxLength(255)] // Specifies the maximum length of the string
        public string Details { get; set; } // Represents the details of the address

        [ForeignKey("User")] // Specifies the foreign key relationship with the "User" entity
        [Required] // Specifies that the following property is required (not nullable)
        public int UserId { get; set; } // Represents the foreign key for the associated user

        public User User { get; set; } // Represents the navigation property for the associated user
    }
}
