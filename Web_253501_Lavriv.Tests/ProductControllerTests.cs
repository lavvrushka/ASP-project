using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Web_253501_Lavriv.Controllers;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;
using WEB_253501_LAVRIV.Controllers;
using WEB_253501_LAVRIV.Services.CategoryService;
using WEB_253501_LAVRIV.Services.ProductService;
using Xunit;

public class ProductControllerTests
{
    [Fact]
    public async Task Index_ReturnsViewWithEmptyModel_WhenCategoryServiceFails()
    {
        // Arrange
        var categoryService = Substitute.For<ICategoryService>();
        var productService = Substitute.For<IProductService>();

        categoryService.GetCategoryListAsync().Returns(Task.FromResult<ResponseData<List<Category>>>(null));

        var controller = CreateController(categoryService, productService);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ListModel<Detail>>(viewResult.Model);
        Assert.Empty(model.Items); // Проверяем, что модель пуста
    }

    [Fact]
    public async Task Index_ReturnsPartialView_WhenProductServiceFails_AjaxRequest()
    {
        // Arrange
        var categoryService = Substitute.For<ICategoryService>();
        var productService = Substitute.For<IProductService>();

        categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>> { Successfull = true, Data = new List<Category>() }));
        productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult<ResponseData<ListModel<Detail>>>(null));

        var controller = CreateController(categoryService, productService, isAjax: true);

        // Act
        var result = await controller.Index();

        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        var model = Assert.IsType<ListModel<Detail>>(partialViewResult.Model);
        Assert.Empty(model.Items);
    }

    [Fact]
    public async Task Index_ReturnsViewWithData_WhenServicesSucceed()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Category1", NormalizedName = "category1" }
        };

        var products = new List<Detail>
        {
            new Detail { Id = 1, Name = "Product1", Price = 100 }
        };

        var categoryService = Substitute.For<ICategoryService>();
        var productService = Substitute.For<IProductService>();

        categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>> { Successfull = true, Data = categories }));
        productService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
        .Returns(Task.FromResult(new ResponseData<ListModel<Detail>>
        {
            Successfull = true,
            Data = new ListModel<Detail>
            {
                Items = products,
                PageNumber = 1, // Установите текущую страницу
                PageSize = 10,  // Установите размер страницы
                TotalCount = 10 // Установите общее количество элементов
            }
        }));


        var controller = CreateController(categoryService, productService);

        // Act
        var result = await controller.Index("category1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ListModel<Detail>>(viewResult.Model);
        Assert.Single(model.Items);
        Assert.Equal(products[0].Name, model.Items.First().Name);
    }

    private ProductController CreateController(
        ICategoryService categoryService,
        IProductService productService,
        bool isAjax = false)
    {
        var controller = new ProductController(productService, categoryService);

        if (isAjax)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
        }
        else
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        controller.TempData = new TempDataDictionary(controller.HttpContext, Substitute.For<ITempDataProvider>());

        return controller;
    }
}
