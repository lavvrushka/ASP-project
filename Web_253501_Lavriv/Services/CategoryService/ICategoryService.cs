using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace WEB_253501_LAVRIV.Services.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
