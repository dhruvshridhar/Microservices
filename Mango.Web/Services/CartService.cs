using System;
using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CartService : BaseService ,ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/AddCart"
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.GET,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/RemoveCart/" + cartDetailsId
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/UpdateCart"
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDto cartDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/ApplyCoupon",
                Data = cartDto
            }) ;
        }

        public async Task<T> RemoveCoupon<T>(string userId, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/RemoveCoupon",
                Data = "dhruv" //will chenge after fixing identity
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDto cartHeaderDto, string? token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/checkout",
                Data = cartHeaderDto//will chenge after fixing identity
            });
        }
    }
}

