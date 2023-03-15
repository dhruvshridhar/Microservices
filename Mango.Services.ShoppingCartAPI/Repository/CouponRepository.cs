using System;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Repository
{
	public class CouponRepository : ICouponRepository
	{
        private readonly HttpClient _httpClient;
        
		public CouponRepository(HttpClient httpClient)
		{
            _httpClient = httpClient;
		}

        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var response = await _httpClient.GetAsync($"/api/coupon/GetCoupon/{couponName}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

            if(content is not null && content.IsSuccess){
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(content.Result));
            }

            return null;
        }
    }
}

