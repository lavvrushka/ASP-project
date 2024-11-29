using WEB_253501_LAVRIV;
using WEB_253501_LAVRIV.Extensions;
using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.FileService;
using WEB_253501_LAVRIV.Services.ProductService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WEB_253501_LAVRIV.HelperClasses;
using WEB_253501_LAVRIV.Services.Authentication;
using Web_253501_Lavriv.Domain.Models;
using WEB_253501_LAVRIV.Logger;
using Serilog;
using Serilog.Events;



var builder = WebApplication.CreateBuilder(args);

builder.RegisterCustomServices();



// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Читаем конфигурацию из appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Используем Serilog как логгер
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
    opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, KeycloakAuthService>();
builder.Services.AddRazorPages();




var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

// Добавляем поддержку сессий
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Добавляем ваши сервисы
builder.Services.AddScoped<Cart, SessionCart>();

builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority =
    $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid"); 
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; 
    options.MetadataAddress =
    $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});




var app = builder.Build();

List<string> errors = new List<string>();
errors.Count();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Подключаем middleware для работы с сессиями
app.UseSession();



// Логирование запросов с Serilog
app.UseSerilogRequestLogging();

// Ваш кастомный middleware для логирования нестандартных кодов состояния
app.UseMiddleware<RequestLoggingMiddleware>();



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();