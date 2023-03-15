using System;
namespace Mango.Web.Services.IServices
{
	public interface ICouponService
	{
        Task<T> GetCoupon<T>(string couponCode); //addd string token too after fixing authorization
    }
}

