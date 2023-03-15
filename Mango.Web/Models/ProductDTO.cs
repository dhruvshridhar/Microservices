using System;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            Count = 1;
        }
        public int productid { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? imageurl { get; set; }
        [Range(1, 100)]
        public int Count { get; set; }
        public string UserId { get; set; }
    }
}

