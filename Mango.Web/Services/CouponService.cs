using System;
using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
	public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetCoupon<T>(string couponCode)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                AccessToken = "",
                ApiType = ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/GetCoupon/" + couponCode
            });
        }
    }
}

