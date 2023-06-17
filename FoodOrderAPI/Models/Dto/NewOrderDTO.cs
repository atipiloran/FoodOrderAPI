using Microsoft.OpenApi.Any;

namespace FoodOrderAPI.Models.Dto
{
    // Represents the request DTO for an order item
    public class OrderItemRequestDTO
    {
        // The ID of the item
        public int ItemId { get; set; }

        // The quantity of the item
        public int Quantity { get; set; }
    }

    // Represents the DTO for creating a new order
    public class NewOrderDTO
    {
        // Additional notes for the order
        public string Notes { get; set; }

        // The ID of the address for delivery
        public int AddressId { get; set; }

        // An array of order items
        public OrderItemRequestDTO[] OrderItems { get; set; }
    }
}
