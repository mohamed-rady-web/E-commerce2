using Ecommerce.Models.User;

namespace Ecommerce.Models.Order
{
    public class CheckOutModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string PaymentMethod { get; set; } = "in Hand";

        public double TotalAmount { get; set; }
        public DateTime CheckOutDate { get; set; }   

        public ICollection<OrderModel> Orders { get; set; }  
        public string Status { get; set; }

    }
}
