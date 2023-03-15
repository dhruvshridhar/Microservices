﻿using System;
using AutoMapper;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repository
{
	public class CouponRepository : ICouponRepository
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
		public CouponRepository(ApplicationDbContext dbContext, IMapper mapper)
		{
            _dbContext = dbContext;
            _mapper = mapper;
		}

        public async Task<CouponDto> GetCouponById(string couponCode)
        {
            var couponFromDb = await _dbContext.coupons.FirstOrDefaultAsync(u => u.CouponCode == couponCode);
            return _mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
