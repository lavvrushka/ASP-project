using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.FileService;
using WEB_253501_LAVRIV.Services.ProductService;

namespace WEB_253501_LAVRIV.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
            builder.Services.AddScoped<IProductService, ApiProductService>();
            builder.Services.AddScoped<IFileService, ApiFileService>();

        }
    }
}
