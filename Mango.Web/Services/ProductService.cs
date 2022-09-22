using System;
using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> CreateProductAsync<T>(ProductDTO product)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.POST,
                Data = product,
                Url = StaticDetails.ProductAPIBase + "/api/products"
            });
        }

        public async Task<T> DeleteProductAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.DELETE,
                Url = StaticDetails.ProductAPIBase + "/api/products/"+id
            });
        }

        public async Task<T> GetAllProductsAsync<T>()
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/products"
            });
        }

        public async Task<T> GetProductById<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/products/"+id
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDTO product)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.PUT,
                Data = product,
                Url = StaticDetails.ProductAPIBase + "/api/products"
            });
        }
    }
}

