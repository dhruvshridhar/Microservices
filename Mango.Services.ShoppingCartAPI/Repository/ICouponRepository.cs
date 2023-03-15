﻿using System;
using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Repository
{
	public interface ICouponRepository
	{
		Task<CouponDto> GetCoupon(string couponName);
	}
}
