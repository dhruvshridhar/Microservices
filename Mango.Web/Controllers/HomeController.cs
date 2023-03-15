using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        List<ProductDTO> products = new();
        var resp = await _productService.GetAllProductsAsync<ResponseDTO>(token);
        if (resp != null && resp.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(resp.Result));
        }

        return View(products);
    }

    public async Task<IActionResult> Details(int productId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        ProductDTO product = new();
        var resp = await _productService.GetProductById<ResponseDTO>(productId,token);
        if (resp != null && resp.IsSuccess)
        {
            product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(resp.Result));
        }

        return View(product);
    }

    [HttpPost]
    [ActionName("Details")]
    public async Task<IActionResult> DetailsPost(ProductDTO productDTO)
    {
        CartDto cartDto = new()
        {
            CartHeader = new CartHeaderDto
            {
                //UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                UserId = productDTO.UserId
            }
        };

        CartDetailsDto cartDetailsDto = new()
        {
            Count = productDTO.Count,
            ProductId = productDTO.productid
        };
        var token = await HttpContext.GetTokenAsync("access_token");
        var resp = await _productService.GetProductById<ResponseDTO>(productDTO.productid,token);
        if(resp is not null && resp.IsSuccess)
        {
            cartDetailsDto.Product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(resp.Result));
        }

        List<CartDetailsDto> cartDetailsDtos = new();
        cartDetailsDtos.Add(cartDetailsDto);
        cartDto.CartDetails = cartDetailsDtos;

        var addToCartResp = await _cartService.AddToCartAsync<ResponseDTO>(cartDto);
        if (addToCartResp is not null && addToCartResp.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(productDTO);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        //var token = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

}

