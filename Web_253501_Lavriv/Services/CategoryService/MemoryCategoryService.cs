using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace WEB_253501_LAVRIV.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
{
                new Category { Id = 1, Name = "Тормозная система", NormalizedName = "brakes" },
                new Category { Id = 2, Name = "Фильтры", NormalizedName = "engine" },
                new Category { Id = 3, Name = "Электроника", NormalizedName = "electronics" },
                new Category { Id = 4, Name = "Освещение", NormalizedName = "lighting" },
                new Category { Id = 5, Name = "Кузовные детали", NormalizedName = "body" },
};
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }

}
