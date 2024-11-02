using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Web_253501_Lavriv.Domain.Entities;
using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.ProductService;

namespace WEB_253501_LAVRIV.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;


        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Detail Detail { get; set; }

        public async Task OnGetAsync()
        {
            
            var response = await _categoryService.GetCategoryListAsync();

           
            if (response?.Data != null)
            {
                ViewData["CategoryId"] = new SelectList(response.Data, "Id", "Name");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(new List<Category>(), "Id", "Name");
            }
        }


        [BindProperty]
        public IFormFile? UploadFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Создание нового объекта с загрузкой файла
            await _productService.CreateProductAsync(Detail, UploadFile);
            return RedirectToPage("./Index");
        }

    }
}