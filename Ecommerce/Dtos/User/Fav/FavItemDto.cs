namespace Ecommerce.Dtos.User.Fav
{
    public class FavItemDto
    {
            public int FavItemId { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string ImageUrl { get; set; }
            public decimal Price { get; set; }
        }
    }
