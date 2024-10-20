using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Web_253501_Lavriv.Domain.Entities;
using WEB_253501_LAVRIV.Services.ProductService;

namespace Web_253501_Lavriv.Pages.Admin.Categories 
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService; 

        public CreateModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Detail Detail { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.CreateProductAsync(Detail, null );

            return RedirectToPage("./Index"); 
        }
    }
}
