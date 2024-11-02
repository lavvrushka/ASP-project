using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_253501_Lavriv.Domain.Entities;
using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.ProductService; // Убедитесь, что используете правильное пространство имен

namespace WEB_253501_LAVRIV.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService; // Используйте интерфейс
        private readonly ICategoryService _categoryService;

        public DeleteModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService; // Внедрение зависимости
            _categoryService = categoryService;
        }

        [BindProperty]
        public Detail Detail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value); // Получаем деталь по ID

            if (response.Data == null)
            {
                return NotFound();
            }
            else
            {
                Detail = response.Data; // Присваиваем значение Detail
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (response.Data != null)
            {
                await _productService.DeleteProductAsync(id.Value); // Метод для удаления
            }

            return RedirectToPage("./Index");
        }
    }
}
