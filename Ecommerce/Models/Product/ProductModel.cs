using Ecommerce.Models.Cart;
using Ecommerce.Models.User.AboutAndContact;
using Ecommerce.Models.User.FavAndBooking;

namespace Ecommerce.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public double Rating { get; set; }
        public ICollection<CartItemModel>cartItems { get; set; }
        public CategoryModel Category { get; set; }
        public int CategoryId { get; set; }
        public SpecialOffersModel SpecialOffer { get; set; }
        public int? SpecialOfferId { get; set; }
        public ICollection<FaveoriteItemModel> FaveoriteItems { get; set; }
        public ICollection<ReviewsModel>Reviews { get; set; }
    }
}
