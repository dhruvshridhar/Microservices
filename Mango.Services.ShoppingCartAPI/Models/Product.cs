using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] //This is done to disable automatically generated PKs and use the id of Product from ProductAPI
        public int productid { get; set; }
        [Required]
        public string name { get; set; }
        [Range(1, 10000)]
        public double price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? imageurl { get; set; }
    }
}

