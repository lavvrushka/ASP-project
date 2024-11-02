using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_253501_Lavriv.Domain.Entities;
using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.ProductService; // Обратите внимание на пространство имен

namespace WEB_253501_LAVRIV.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService; // Используем IProductService вместо DbContext
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public IFormFile? UploadFile { get; set; } // Новый параметр для загрузки файла
        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
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

            var response = await _productService.GetProductByIdAsync(id.Value); // Замените метод на ваш метод из сервиса
            if (response.Data == null)
            {
                return NotFound();
            }
            Detail = response.Data; // Присваиваем полученные данные

            // Заполняем список категорий
            var categories = await _categoryService.GetCategoryListAsync(); // Получите список категорий
            ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Передаём id, объект Detail и файл
                await _productService.UpdateProductAsync(Detail.Id, Detail, UploadFile);
            }
            catch (Exception)
            {
                if (!await DetailExists(Detail.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private async Task<bool> DetailExists(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return response.Data != null; // Проверяем, существует ли продукт
        }
    }
}
