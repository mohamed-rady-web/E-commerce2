namespace Ecommerce.Dtos.User.Fav
{
    public class FavDto
    {
            public int Id { get; set; }
        
            public string UserId { get; set; }
            public ICollection<FavItemDto> Items { get; set; }
            public string Message { get; set; }
        }
    }

