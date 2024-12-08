using Xunit;
using Moq;
using ECommerceWebApplication.Controllers;
using ECommerceWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ECommerceWebApplication.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerceWebApplicationTests
{
    public class ProductsControllerTests
    {
        private ProductsController CreateController(ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            return new ProductsController(dbContext, environment);
        }

        [Fact]
        public void Index_ReturnsViewWithProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("IndexTestDb")
                .Options;

            using var dbContext = new ApplicationDbContext(options);
            dbContext.Products.Add(new Product { Name = "Test Product 1" });
            dbContext.Products.Add(new Product { Name = "Test Product 2" });
            dbContext.SaveChanges();

            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var controller = CreateController(dbContext, mockEnvironment.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }


        [Fact]
        public void Create_Get_ReturnsView()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CreateGetTestDb")
                .Options;

            using var dbContext = new ApplicationDbContext(options);
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var controller = CreateController(dbContext, mockEnvironment.Object);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }





        [Fact]
        public void Edit_Get_ValidId_ReturnsViewWithProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EditGetTestDb")
                .Options;

            using var dbContext = new ApplicationDbContext(options);
            var product = new Product
            {
                Id = 1,
                Name = "Test Product"
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var controller = CreateController(dbContext, mockEnvironment.Object);

            // Act
            var result = controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductDto>(viewResult.Model);
            Assert.Equal("Test Product", model.Name);
        }




        [Fact]
        public void Edit_Post_ValidData_RedirectsToIndex()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EditPostTestDb")
                .Options;

            using var dbContext = new ApplicationDbContext(options);
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                ImageFileName = "test.png"
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(e => e.WebRootPath).Returns("wwwroot");

            var controller = CreateController(dbContext, mockEnvironment.Object);

            var productDto = new ProductDto
            {
                Name = "Updated Product",
                Brand = "Updated Brand",
                Category = "Updated Category",
                Price = 150,
                Description = "Updated Description",
                ImageFile = null
            };

            // Act
            var result = controller.Edit(1, productDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var updatedProduct = dbContext.Products.Find(1);
            Assert.Equal("Updated Product", updatedProduct.Name);
            Assert.Equal(150, updatedProduct.Price);
        }

    }
}