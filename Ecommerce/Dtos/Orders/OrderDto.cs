using Ecommerce.Models.Order;
using Ecommerce.Models.User;

namespace Ecommerce.Dtos.Orders
{
    public class OrderDto
    {
        public string Message { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

    }
}
