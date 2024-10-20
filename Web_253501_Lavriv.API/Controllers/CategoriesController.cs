using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_253501_Lavriv.API.Services.CategoryService;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace Web_253501_Lavriv.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var response = await _categoryService.GetCategoryListAsync();

            if (!response.Successfull)
            {
                // Вернуть статус 500 и сообщение об ошибке
                return StatusCode(500, response.ErrorMessage);
            }

            return Ok(response);
        }

    }
}
