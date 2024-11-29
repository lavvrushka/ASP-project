using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_253501_Lavriv.API.Data;
using Web_253501_Lavriv.API.Models;
using Web_253501_Lavriv.API.Services.CategoryService;
using Web_253501_Lavriv.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();


// Добавление CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7217")  // Указываем разрешённый источник
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});




var authServer = builder.Configuration
.GetSection("AuthServer")
.Get<AuthServerData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
   
    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
   
    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
    
    o.Audience = "account";
    
  
    o.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

var app = builder.Build();

app.UseStaticFiles();

await DbInitializer.SeedData(app);


app.UseSwagger();
app.UseSwaggerUI();

// Применение CORS политики
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();