using System;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductById<T>(int id, string token);
        Task<T> CreateProductAsync<T>(ProductDTO product, string token);
        Task<T> UpdateProductAsync<T>(ProductDTO product, string token);
        Task<T> DeleteProductAsync<T>(int id, string token);

    }
}

