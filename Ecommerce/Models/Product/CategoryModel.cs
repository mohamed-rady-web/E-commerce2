﻿using Ecommerce.Dtos.Products;

namespace Ecommerce.Models.Product
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<ProductModel> Products { get; set; }
    }
}
