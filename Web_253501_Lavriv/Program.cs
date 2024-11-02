using WEB_253501_LAVRIV;
using WEB_253501_LAVRIV.Extensions;
using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.FileService;
using WEB_253501_LAVRIV.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCustomServices();

// Добавляем Razor Pages и контроллеры с представлениями
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));

var app = builder.Build();

List<string> errors = new List<string>();
errors.Count();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Добавляем маршрут для Razor Pages
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
