using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Messages;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDTO _responseDTO;
        private readonly IMessageBus _messageBus;
        private readonly ICouponRepository _couponRepository;

        public CartController(ICartRepository cartRepository, IMessageBus messageBus, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _responseDTO = new ResponseDTO();
            _messageBus = messageBus;
            _couponRepository = couponRepository;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(userId);
                _responseDTO.Result = cartDto;
            }
            catch(Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart([FromBody] CartDto cartDto )
        {
            try
            {
                await _cartRepository.CreateUpdateCart(cartDto);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart([FromBody] CartDto cartDto)
        {
            try
            {
                await _cartRepository.CreateUpdateCart(cartDto);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }

        [HttpPost("RemoveCart/{cartId}")]
        public async Task<object> RemoveCart(int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
                _responseDTO.Result = isSuccess;
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }

        [HttpPost("ClearCart")]
        public async Task<object> ClearCart([FromBody] string cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.ClearCart(cartId);
                _responseDTO.Result = isSuccess;
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                bool isSuccess = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
                _responseDTO.Result = isSuccess;
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveCoupon(userId);
                _responseDTO.Result = isSuccess;
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout([FromBody] CheckoutHeaderDto checkoutHeaderDto)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(checkoutHeaderDto.UserId);

                if(cartDto is null)
                {
                    return BadRequest();
                }

                // To check if coupon is still valid and recalculate if needed
                if (!string.IsNullOrEmpty(checkoutHeaderDto.CouponCode))
                {
                    var coupon = await _couponRepository.GetCoupon(checkoutHeaderDto.CouponCode);

                    if (checkoutHeaderDto.DiscountTotal != coupon.DiscountAmount)
                    {
                        _responseDTO.IsSuccess = false;
                        _responseDTO.Errors = new List<string>() { "Discount has changed, please re-apply" };
                        return _responseDTO;
                    }
                }

                checkoutHeaderDto.CartDetails = cartDto.CartDetails;

                //send message to service bus
                await _messageBus.PublishMessage(checkoutHeaderDto, "checkoutmessagetopic");
                await _cartRepository.ClearCart(checkoutHeaderDto.UserId);
            }
            catch (Exception e)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Errors = new List<string>() { e.ToString() };
            }
            return _responseDTO;
        }
    }
}

