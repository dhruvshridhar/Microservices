﻿using System;
using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserId(string userId);
        Task<CartDto> CreateUpdateCart(CartDto cart);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ClearCart(string userId);
        Task<bool> ApplyCoupon(string userId, string code);
        Task<bool> RemoveCoupon(string userId);
    }
}
