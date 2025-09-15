namespace Ecommerce.Dtos.Orders
{
    public class AddOrderItemDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

    }
}
