using Microsoft.AspNetCore.Mvc;
using Web_253501_Lavriv.Domain.Entities;
using WEB_253501_LAVRIV.Services.CategoryService;
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

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (!categoryResponse.Successfull)
            {
                return NotFound(categoryResponse.ErrorMessage);
            }

            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
            {
                return NotFound(productResponse.ErrorMessage);
            }

            ViewBag.Categories = categoryResponse.Data;
            ViewBag.PageNo = pageNo;
            ViewBag.TotalCount = productResponse.Data.TotalCount; // Просто присваиваем значение

            return View(productResponse.Data.Items);
        }


    }
}
