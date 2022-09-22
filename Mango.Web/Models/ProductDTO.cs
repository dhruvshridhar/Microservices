using System;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDTO
    {
        public int productid { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? imageurl { get; set; }
    }
}

