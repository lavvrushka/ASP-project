using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web_253501_Lavriv.Domain.Entities;
using WEB_253501_LAVRIV.Services.ProductService;

namespace WEB_253501_LAVRIV.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public List<Detail> Products { get; set; } = new List<Detail>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNo = 1)
        {
            CurrentPage = pageNo;

            // Получаем список продуктов для текущей страницы
            var response = await _productService.GetProductListAsync(null, pageNo);

            if (response.Data != null)
            {
                Products = response.Data?.Items ?? new List<Detail>();
                int totalItems = response.Data?.TotalCount ?? 0; // Используйте TotalCount вместо TotalItems
                int itemsPerPage = 3; // установите ваше значение, используемое для пагинации
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
            }
        }
    }
}
