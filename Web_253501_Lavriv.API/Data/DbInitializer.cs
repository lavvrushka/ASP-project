using Microsoft.EntityFrameworkCore;
using Web_253501_Lavriv.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
namespace Web_253501_Lavriv.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            await context.Database.MigrateAsync();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Тормоза", NormalizedName = "brakes" },
                    new Category { Name = "Двигатель", NormalizedName = "engine" },
                    new Category { Name = "Электроника", NormalizedName = "electronics" },
                    new Category { Name = "Освещение", NormalizedName = "lighting" },
                    new Category { Name = "Кузовные детали", NormalizedName = "body" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
            var baseUrl = app.Configuration["AppUrl"];

            if (!context.Details.Any())
            {
                var details = new List<Detail>
                    {
                        new Detail { Name = "Тормозные колодки", Description = "Высококачественные тормозные колодки", Price = 50.99M, Image = $"{baseUrl}Images/BrakePads.jpg", Category = context.Categories.First(c => c.NormalizedName == "brakes") },
                        new Detail { Name = "Масляный фильтр", Description = "Фильтр для двигателей", Price = 15.99M, Image = $"{baseUrl}Images/oilFilter.jpg", Category = context.Categories.First(c => c.NormalizedName == "engine") },
                        new Detail { Name = "Аккумулятор 12V", Description = "Батарея с высоким сроком службы", Price = 120.50M, Image = $"{baseUrl}Images/Battery.jpg", Category = context.Categories.First(c => c.NormalizedName == "electronics") },
                        new Detail { Name = "Свечи зажигания", Description = "Высококачественные свечи зажигания для бензиновых двигателей", Price = 10.99M, Image = $"{baseUrl}Images/SparkPlugs.jpg", Category = context.Categories.First(c => c.NormalizedName == "engine") },
                        new Detail { Name = "Тормозные диски", Description = "Надежные тормозные диски для легковых автомобилей", Price = 75.00M, Image = $"{baseUrl}Images/BrakeDiscs.jpg", Category = context.Categories.First(c => c.NormalizedName == "brakes") },
                        new Detail { Name = "Фара головного света", Description = "Энергосберегающая фара для вашего автомобиля", Price = 45.50M, Image = $"{baseUrl}Images/Headlight.jpg", Category = context.Categories.First(c => c.NormalizedName == "electronics") },
                        new Detail { Name = "Автомобильные щетки", Description = "Щетки для очистки лобового стекла, долговечные и эффективные", Price = 25.00M, Image = $"{baseUrl}Images/Wipers.jpg", Category =      context.Categories.First(c => c.NormalizedName == "electronics") },
                        new Detail { Name = "Магнитола", Description = "Современная автомобильная магнитола с Bluetooth и USB", Price = 99.99M, Image = $"{baseUrl}Images/Stereo.jpg", Category = context.Categories.First(c => c.NormalizedName == "electronics") },
                        new Detail { Name = "Парктроник", Description = "Устройство для помощи при парковке с датчиками", Price = 55.00M, Image = $"{baseUrl}Images/ParkingSensor.jpg", Category = context.Categories.First(c => c.NormalizedName == "electronics") },
                        new Detail { Name = "Камера заднего вида", Description = "Камера для улучшения видимости при заднем ходу", Price = 75.00M, Image = $"{baseUrl}Images/RearCamera.jpg", Category = context.Categories.First(c => c.NormalizedName == "electronics") },
                        new Detail { Name = "LED лампа для фар", Description = "Энергоэффективные LED лампы для автомобиля", Price = 30.00M, Image = $"{baseUrl}Images/LEDHeadlight.jpg", Category = context.Categories.First(c => c.NormalizedName == "lighting") },
                        new Detail { Name = "Автомобильные габариты", Description = "Габаритные огни для повышения видимости на дороге", Price = 18.00M, Image = $"{baseUrl}Images/MarkerLights.jpg", Category = context.Categories.First(c => c.NormalizedName == "lighting") },
                        new Detail { Name = "Капот", Description = "Капот для легкового автомобиля", Price = 150.00M, Image = $"{baseUrl}Images/Bonnet.jpg", Category = context.Categories.First(c => c.NormalizedName == "body") },
                        new Detail { Name = "Бамперы", Description = "Передний и задний бамперы для различных моделей", Price = 200.00M, Image = $"{baseUrl}Images/Bumper.jpg", Category = context.Categories.First(c => c.NormalizedName == "body") }
                    };

                await context.Details.AddRangeAsync(details);
                await context.SaveChangesAsync();
            }

        }


    }

}
