using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;
using Web_253501_Lavriv.API.Data;
using Web_253501_Lavriv.API.Services.ProductService;
using Xunit;

namespace Web_253501_Lavriv.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Добавляем тестовые данные
            context.Details.AddRange(
                new Detail { Id = 1, Name = "Product1", CategoryId = 1 },
                new Detail { Id = 2, Name = "Product2", CategoryId = 1 },
                new Detail { Id = 3, Name = "Product3", CategoryId = 1 },
                new Detail { Id = 4, Name = "Product4", CategoryId = 2 },
                new Detail { Id = 5, Name = "Product5", CategoryId = 2 }
            );
            context.Categories.AddRange(
                new Category { Id = 1, Name = "Category1", NormalizedName = "category1" },
                new Category { Id = 2, Name = "Category2", NormalizedName = "category2" }
            );
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new ProductService(context); // Исправлено: только 1 параметр

            var result = await service.GetProductListAsync(null);

            Assert.IsType<ResponseData<ListModel<Detail>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.PageNumber);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages); // 5 товаров, 2 страницы по 3
            Assert.Equal("Product1", result.Data.Items[0].Name);
        }

        [Fact]
        public async Task ServiceReturnsSecondPageOfItems()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            var result = await service.GetProductListAsync(null, pageNo: 2);

            Assert.True(result.Successfull);
            Assert.Equal(2, result.Data.PageNumber);
            Assert.Equal(2, result.Data.Items.Count); // Осталось 2 товара на второй странице
            Assert.Equal("Product4", result.Data.Items[0].Name);
        }

        [Fact]
        public async Task ServiceFiltersItemsByCategory()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            var result = await service.GetProductListAsync("category1");

            Assert.True(result.Successfull);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.All(result.Data.Items, p => Assert.Equal(1, p.CategoryId));
        }

        [Fact]
        public async Task ServiceRestrictsPageSizeToMaximum()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            // Задаём слишком большой размер страницы
            var result = await service.GetProductListAsync(null, pageSize: 100);

            Assert.True(result.Successfull);
            Assert.Equal(3, result.Data.PageSize); // Максимальный размер страницы 3
            Assert.Equal(3, result.Data.Items.Count);
        }

        [Fact]
        public async Task ServiceReturnsErrorWhenPageNumberExceedsTotalPages()
        {
            using var context = CreateContext();
            var service = new ProductService(context);

            var result = await service.GetProductListAsync(null, pageNo: 10);

            Assert.False(result.Successfull);
            Assert.Equal("Page number exceeds total pages", result.ErrorMessage);
        }
    }
}
