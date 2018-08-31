using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTest
    {
        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                    new Product() { ProductID = 1, Name = "p1"},
                    new Product() { ProductID = 2, Name = "p2"},
                    new Product() { ProductID = 3, Name = "p3"},
                    new Product() { ProductID = 4, Name = "p4"},
                    new Product() { ProductID = 5, Name = "p5"}
                }).AsQueryable<Product>());
                
           
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 2;

            ProductListViewModel result = controller.List(2).ViewData.Model as ProductListViewModel;

            Product[] products = result.Products.ToArray();
            
            Assert.True(products.Length == 2);
            Assert.Equal("p3", products[0].Name);
            Assert.Equal("p4", products[1].Name);
        }
    }
}