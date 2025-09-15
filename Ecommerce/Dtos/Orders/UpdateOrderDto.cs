namespace Ecommerce.Dtos.Orders
{
    public class UpdateOrderDto
    {   
        public string Status { get; set; }="Pending";
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }

    }
}
