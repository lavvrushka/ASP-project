using Microsoft.AspNetCore.Mvc;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;
using WEB_253501_LAVRIV.Extensions;
using WEB_253501_LAVRIV.Services.CategoryService;
using Microsoft.Extensions.Logging;  // Убедитесь, что этот using присутствует
using WEB_253501_LAVRIV.Services.ProductService;

namespace WEB_253501_LAVRIV.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [Route("Catalog/{category?}")]
        public async Task<IActionResult> Index(string? category = null, int pageNo = 1)
        {
         
            var response = await _categoryService.GetCategoryListAsync();

            if (response == null || response.Data == null)
            {

                return View(new ListModel<Detail>());
            }

            var categories = response.Data;

            var currentCategory = categories.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";
            ViewData["categories"] = categories;
            ViewData["currentCategory"] = currentCategory;
            ViewData["currentCategoryNormalizedName"] = category;

            var products = await _productService.GetProductListAsync(category, pageNo);

            if (products == null || products.Data == null)
            {

                var emptyModel = new ListModel<Detail>();

                if (Request.IsAjaxRequest())  // Check if the request is AJAX
                {
 
                    return PartialView("_ListPartial", emptyModel);
                }

                return View(emptyModel);
            }

            var model = products.Data;

            if (Request.IsAjaxRequest())  // Check if the request is AJAX
            {
                return PartialView("_ListPartial", model);
            }

           
            return View(model);
        }



    }
}
