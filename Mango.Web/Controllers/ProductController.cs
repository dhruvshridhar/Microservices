using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO> items = new();
            var response = await _productService.GetAllProductsAsync<ResponseDTO>();

            if(response is not null && response.IsSuccess)
            {
                items = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(items);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var resp = await _productService.CreateProductAsync<ResponseDTO>(product);

                if (resp != null && resp.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int productid)
        {
            var resp = await _productService.GetProductById<ResponseDTO>(productid);

            if (resp != null && resp.IsSuccess)
            {
                var item = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(resp.Result));
                return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var resp = await _productService.UpdateProductAsync<ResponseDTO>(product);

                if (resp != null && resp.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(product);
        }


        public async Task<IActionResult> ProductDelete(int productid)
        {
            var resp = await _productService.GetProductById<ResponseDTO>(productid);

            if (resp != null && resp.IsSuccess)
            {
                var item = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(resp.Result));
                return View(item);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var resp = await _productService.DeleteProductAsync<ResponseDTO>(model.productid);

                if (resp.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
    }
}

