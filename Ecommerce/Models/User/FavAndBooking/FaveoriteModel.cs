using Ecommerce.Models.Product;

namespace Ecommerce.Models.User.FavAndBooking
{
    public class FaveoriteModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime AddedDate { get; set; }
        public ICollection<FaveoriteItemModel> Items { get; set; }
    }
}
