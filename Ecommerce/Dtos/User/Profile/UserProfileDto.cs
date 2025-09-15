using Ecommerce.Dtos.Cart;
using Ecommerce.Dtos.CheckOut;
using Ecommerce.Dtos.Orders;
using Ecommerce.Dtos.User.Fav;
using Ecommerce.Dtos.User.Reservation;
using Ecommerce.Dtos.User.Reviews;

namespace Ecommerce.Dtos.User.Profile
{
    public class UserProfileDto
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CartId { get; set; }
        public CartDto Cart { get; set; }

        public int FavoriteId { get; set; }
        public FavDto Favorite { get; set; }

        public int CheckOutId { get; set; }
        public ICollection<CheckOutDto> CheckOuts { get; set; }

        public ICollection<OrderDto> Orders { get; set; }
        public ICollection<ReviewsDto> Reviews { get; set; }
        public ICollection<BookingDto> Bookings { get; set; }

        public string Message { get; set; }
    }
}
