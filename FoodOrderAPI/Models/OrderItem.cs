using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderAPI.Models
{
    public class OrderItem
    {
        [Key] // Specifies that the following property is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies that the database will generate the value for this property
        public int Id { get; set; } // Represents the unique identifier for the order item

        [ForeignKey("OrderHeader")] // Specifies the foreign key relationship with the OrderHeader entity
        [Required] // Specifies that the following property is required (not nullable)
        public int OrderHeaderId { get; set; } // Represents the foreign key reference to the OrderHeader entity
        public OrderHeader OrderHeader { get; set; } // Represents the navigation property for the related OrderHeader entity

        [ForeignKey("Item")] // Specifies the foreign key relationship with the Item entity
        [Required] // Specifies that the following property is required (not nullable)
        public int ItemId { get; set; } // Represents the foreign key reference to the Item entity
        public Item Item { get; set; } // Represents the navigation property for the related Item entity

        [Required] // Specifies that the following property is required (not nullable)
        public int Quantity { get; set; } // Represents the quantity of the item in the order
    }
}
