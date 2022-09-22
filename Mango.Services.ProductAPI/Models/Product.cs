using System;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public int productid { get; set; }
        [Required]
        public string name { get; set; }
        [Range(1,10000)]
        public double price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? imageurl { get; set; }
    }
}

