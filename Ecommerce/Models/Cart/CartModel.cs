using Ecommerce.Models.User;

namespace Ecommerce.Models.Cart
{
    public class CartModel
    {
        public int Id { get; set; }
       public string UserId { get; set; }
       public ApplicationUser User { get; set; }
        public ICollection<CartItemModel> CartItems { get; set; } 
    }
}
