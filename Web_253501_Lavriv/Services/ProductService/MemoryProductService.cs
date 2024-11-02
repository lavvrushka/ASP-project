using Microsoft.Extensions.Configuration;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;
using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.ProductService;

public class MemoryProductService : IProductService
{
    private List<Detail> _details;
    private List<Category> _categories;
    private readonly int _pageSize;

    public MemoryProductService(IConfiguration config, ICategoryService categoryService)
    {
        _pageSize = config.GetValue<int>("ItemsPerPage");
        InitializeCategoriesAsync(categoryService).Wait();
        SetupData();
    }

    private async Task InitializeCategoriesAsync(ICategoryService categoryService)
    {
        var categoryResponse = await categoryService.GetCategoryListAsync();
        if (categoryResponse.Successfull)
        {
            _categories = categoryResponse.Data;
        }
        else
        {
            _categories = new List<Category>();
        }
    }

    private void SetupData()
    {
        if (_categories == null || !_categories.Any())
        {
            throw new InvalidOperationException("Categories are not initialized.");
        }

        _details = new List<Detail>
{
            //    new Detail { Id = 1, Name = "Тормозные колодки", Description = "Высококачественные тормозные колодки для автомобилей среднего класса", Price = 50.99M, Image = "Images/BrakePads.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("brakes")) },
            //    new Detail { Id = 2, Name = "Масляный фильтр", Description = "Фильтр для двигателей с высокой производительностью", Price = 15.99M, Image = "Images/OilFilter.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("engine")) },
            //    new Detail { Id = 3, Name = "Аккумулятор 12V", Description = "Аккумуляторная батарея с высоким сроком службы", Price = 120.50M, Image = "Images/Battery.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("electronics")) },
            //    new Detail { Id = 4, Name = "Свечи зажигания", Description = "Высококачественные свечи зажигания для бензиновых двигателей", Price = 10.99M, Image = "Images/SparkPlugs.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("engine")) },
            //    new Detail { Id = 5, Name = "Тормозные диски", Description = "Надежные тормозные диски для легковых автомобилей", Price = 75.00M, Image = "Images/BrakeDiscs.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("brakes")) },
            //    new Detail { Id = 6, Name = "Фара головного света", Description = "Энергосберегающая фара для вашего автомобиля", Price = 45.50M, Image = "Images/Headlight.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("electronics")) },
            //    new Detail { Id = 7, Name = "Автомобильные щетки", Description = "Щетки для очистки лобового стекла, долговечные и эффективные", Price = 25.00M, Image = "Images/Wipers.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("electronics")) },
            //    new Detail { Id = 8, Name = "Магнитола", Description = "Современная автомобильная магнитола с Bluetooth и USB", Price = 99.99M, Image = "Images/Stereo.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("electronics")) },
            //    new Detail { Id = 9, Name = "Парктроник", Description = "Устройство для помощи при парковке с датчиками", Price = 55.00M, Image = "Images/ParkingSensor.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("electronics")) },
            //    new Detail { Id = 10, Name = "Камера заднего вида", Description = "Камера для улучшения видимости при заднем ходу", Price = 75.00M, Image = "Images/RearCamera.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("electronics")) },
            //     // Товары в категории "Освещение"
            //    new Detail { Id = 11, Name = "LED лампа для фар", Description = "Энергоэффективные LED лампы для автомобиля", Price = 30.00M, Image = "Images/LEDHeadlight.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("lighting")) },
            //    new Detail { Id = 12, Name = "Автомобильные габариты", Description = "Габаритные огни для повышения видимости на дороге", Price = 18.00M, Image = "Images/MarkerLights.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("lighting")) },

            //    // Товары в категории "Кузовные детали"
            //    new Detail { Id = 13, Name = "Капот", Description = "Капот для легкового автомобиля", Price = 150.00M, Image = "Images/Bonnet.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("body")) },
            //    new Detail { Id = 14, Name = "Бамперы", Description = "Передний и задний бамперы для различных моделей", Price = 200.00M, Image = "Images/Bumper.jpg", MimeType = "image/jpeg", Category = _categories.Find(c => c.NormalizedName.Equals("body")) }

        };
    }

    public async Task<ResponseData<ListModel<Detail>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var filteredDetails = string.IsNullOrEmpty(categoryNormalizedName)
            ? _details
            : _details.Where(d => d.Category.NormalizedName.Equals(categoryNormalizedName, StringComparison.OrdinalIgnoreCase)).ToList();

        var pagedDetails = filteredDetails
            .Skip((pageNo - 1) * _pageSize)
            .Take(_pageSize)
            .ToList();

        var listModel = new ListModel<Detail>
        {
            Items = pagedDetails,
            TotalCount = filteredDetails.Count
        };

        return new ResponseData<ListModel<Detail>> { Data = listModel, Successfull = true };
    }

    public Task<ResponseData<Detail>> GetProductByIdAsync(int id)
    {
        var detail = _details.FirstOrDefault(d => d.Id == id);
        return Task.FromResult(new ResponseData<Detail> { Data = detail, Successfull = detail != null });
    }

    public Task UpdateProductAsync(int id, Detail product, IFormFile? formFile) => Task.CompletedTask;
    public Task DeleteProductAsync(int id) => Task.CompletedTask;
    public Task<ResponseData<Detail>> CreateProductAsync(Detail product, IFormFile? formFile) => Task.FromResult(new ResponseData<Detail> { Data = product, Successfull = true });
}
