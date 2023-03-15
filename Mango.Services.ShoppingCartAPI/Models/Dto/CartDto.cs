using System;
namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class CartDto
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}

