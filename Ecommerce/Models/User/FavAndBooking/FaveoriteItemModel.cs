using Ecommerce.Models.Product;
using System;

namespace Ecommerce.Models.User.FavAndBooking
{
    public class FaveoriteItemModel
    {
       public int Id { get; set; }
       public DateTime AddedDate { get; set; }
        public int FavoriteId { get; set; }
        public FaveoriteModel Favorite { get; set; }

        public int ProductId { get; set; }
        public ProductModel Product { get; set; }

    }
}
