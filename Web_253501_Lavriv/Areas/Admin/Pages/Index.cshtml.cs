using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web_253501_Lavriv.Domain.Entities; // Убедитесь, что используете правильное пространство имен
using WEB_253501_LAVRIV.Services.ProductService; // Убедитесь, что используете правильное пространство имен

namespace Web_253501_Lavriv.Pages.Admin.Categories // Обновите путь, если нужно
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService; // Используйте интерфейс

        public IndexModel(IProductService productService)
        {
            _productService = productService; // Внедрение зависимости
        }

        public List<Detail> Products { get; set; } // Теперь это список Detail

        public async Task OnGetAsync()
        {
            // Замените null и 1 на реальные значения, если необходимо
            var response = await _productService.GetProductListAsync(null, 1); // Используйте метод GetProductListAsync

            // Присваиваем Products значения из Items
            Products = response.Data?.Items ?? new List<Detail>(); // Используйте Items из ListModel
        }
    }
}
