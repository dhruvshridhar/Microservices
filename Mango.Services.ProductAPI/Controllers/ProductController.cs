using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Get all products
        [HttpGet]
        [Authorize]
        public async Task<ResponseDTO> GetAllProducts()
        {
            var resp = new ResponseDTO();

            try
            {
                var products = await _productRepository.GetProducts();

                resp.IsSuccess = true;
                resp.Message = "Success";
                resp.Result = products;

                return resp;
            }
            catch(Exception e)
            {
                resp.IsSuccess = false;
                resp.Message = e.Message;
                return resp;
            }
        }

        // Add or update Product
        [HttpPost]
        [Authorize]
        public async Task<ResponseDTO> CreateUpdateProduct([FromBody] ProductDTO product)
        {
            var resp = new ResponseDTO();
            try
            {
                resp.Result = await _productRepository.CreateProduct(product);
                resp.IsSuccess = true;
                resp.Message = "Success";

                return resp;
            }
            catch(Exception e)
            {
                resp.IsSuccess = false;
                resp.Message = e.Message;
                return resp;
            }
        }

        // Get product by id
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ResponseDTO> GetProductById(int id)
        {
            var resp = new ResponseDTO();
            try
            {
                resp.Result = await _productRepository.GetProductById(id);
                resp.IsSuccess = true;
                resp.Message = "Success";

                return resp;
            }
            catch(Exception e)
            {
                resp.IsSuccess = false;
                resp.Message = e.Message;
                return resp;
            }
        }

        // Delete by id
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ResponseDTO> DeleteProduct(int id)
        {
            var resp = new ResponseDTO();
            try
            {
                resp.Result = await _productRepository.DeleteProduct(id);
                resp.IsSuccess = true;
                resp.Message = "Success";

                return resp;
            }
            catch (Exception e)
            {
                resp.IsSuccess = false;
                resp.Message = e.Message;
                return resp;
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseDTO> UpdateProduct([FromBody] ProductDTO product)
        {
            var resp = new ResponseDTO();
            try
            {
                resp.Result = await _productRepository.CreateProduct(product);
                resp.IsSuccess = true;
                resp.Message = "Success";

                return resp;
            }
            catch (Exception e)
            {
                resp.IsSuccess = false;
                resp.Message = e.Message;
                return resp;
            }
        }
    }
}

