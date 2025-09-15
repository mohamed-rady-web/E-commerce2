namespace Ecommerce.Dtos.Orders
{
    public class CreateOrderDto
    {
        public ICollection<AddOrderItemDto>Items { get; set; }
    }
}
