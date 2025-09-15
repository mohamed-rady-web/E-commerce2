namespace Ecommerce.Dtos.Products
{
    public class UpdateProductDto
    {
        
            public string? Name { get; set; }
            public string ?Description { get; set; }
            public decimal ?Price { get; set; }
            public string ?ImageUrl { get; set; }
            public int ?Stock { get; set; }
            public double? Rating { get; set; }

       
            public int? CategoryId { get; set; }
            public int? SpecialOfferId { get; set; }
        }

    }
