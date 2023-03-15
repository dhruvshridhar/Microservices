using System;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Repository
{
	public interface ICouponRepository
	{
		Task<CouponDto> GetCouponById(string couponCode);
	}
}

