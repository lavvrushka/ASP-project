using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_253501_Lavriv.Domain.Models;
using WEB_253501_LAVRIV.Services.ProductService;

namespace WEB_253501_LAVRIV.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly Cart _cart;

        public CartController(IProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart; // Внедрённая корзина
        }

        [HttpPost("add/{id:int}")]
        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            // Получаем данные о продукте
            var productResponse = await _productService.GetProductByIdAsync(id);
            if (productResponse.Successfull && productResponse.Data != null)
            {
                _cart.AddToCart(productResponse.Data); // Добавляем в корзину
            }

            return Redirect(returnUrl);
        }

        [HttpPost("remove/{id:int}")]
        public IActionResult Remove(int id, string returnUrl)
        {
            _cart.RemoveItems(id); // Удаляем из корзины
            return Redirect(returnUrl);
        }

        [HttpGet("view")]
        public IActionResult ViewCart()
        {
            return View(_cart); // Передаём корзину в представление
        }
    }
}
