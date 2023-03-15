using System;
namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string Message { get; set; } = "";
        public List<string> Errors { get; set; }
    }
}

