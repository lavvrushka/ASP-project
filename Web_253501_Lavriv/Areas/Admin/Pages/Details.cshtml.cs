using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253501_Lavriv.Domain.Entities;
using WEB_253501_LAVRIV.Services.ProductService; // Убедитесь, что используете правильное пространство имен

namespace WEB_253501_LAVRIV.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService; // Используем IProductService

        public DetailsModel(IProductService productService)
        {
            _productService = productService; // Внедрение зависимости
        }

        public Detail Detail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (response.Data == null)
            {
                Console.WriteLine("No data retrieved for the product ID: " + id); // Сообщение, если данных нет
                return NotFound();
            }
            else
            {
                Detail = response.Data;
                Console.WriteLine($"Product loaded: {Detail.Name}, Image: {Detail.Image}"); // Проверка загрузки данных
            }
            return Page();
        }

    }
}
