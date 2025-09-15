using Ecommerce.Models.Cart;
using Ecommerce.Models.Order;
using Ecommerce.Models.User.AboutAndContact;
using Ecommerce.Models.User.FavAndBooking;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.User
{
    public class ApplicationUser: IdentityUser
    {
        [Required,MaxLength(20),MinLength(2)]
        public string FirstName { get; set; }
        [Required, MaxLength(20), MinLength(2)]
        public string LastName { get; set; }
       
        public FaveoriteModel Faveorites { get; set; }
        public CartModel Cart { get; set; }
        public int CartId { get; set; }
        public int FaveoriteId { get; set; }
        public int CheckOutId { get; set; }
        public ICollection<CheckOutModel> CheckOuts { get; set; }
        public ICollection<OrderModel>Orders { get; set; }
        public ICollection<ReviewsModel> Reviews { get; set; }
        public ICollection<BookingModel> Bookings { get; set; }
    }
}
