using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderAPI.Models
{
    public class OrderHeader
    {
        [Key] // Specifies that the following property is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies that the database will generate the value for this property
        public int Id { get; set; } // Represents the unique identifier for the order header

        [Required] // Specifies that the following property is required (not nullable)
        [MaxLength(255)] // Specifies the maximum length of the string
        public string Notes { get; set; } // Represents additional notes for the order

        [ForeignKey("User")] // Specifies the foreign key relationship with the User entity
        [Required] // Specifies that the following property is required (not nullable)
        public int UserId { get; set; } // Represents the foreign key reference to the User entity
        public User User { get; set; } // Represents the navigation property for the related User entity

        [ForeignKey("Address")] // Specifies the foreign key relationship with the Address entity
        [Required] // Specifies that the following property is required (not nullable)
        public int AddressId { get; set; } // Represents the foreign key reference to the Address entity
        public Address Address { get; set; } // Represents the navigation property for the related Address entity

        [Required] // Specifies that the following property is required (not nullable)
        public DateTime DateTime { get; set; } // Represents the date and time of the order

        [Required] // Specifies that the following property is required (not nullable)
        public int Status { get; set; } // Represents the status of the order

        // Status values:
        // 0 - Canceled
        // 1 - New Order
        // 2 - Confirmed
        // 3 - In Transit
        // 4 - Delivered
    }
}
