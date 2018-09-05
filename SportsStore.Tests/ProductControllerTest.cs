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
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "p1"},
                new Product() {ProductID = 2, Name = "p2"},
                new Product() {ProductID = 3, Name = "p3"},
                new Product() {ProductID = 4, Name = "p4"},
                new Product() {ProductID = 5, Name = "p5"}
            }).AsQueryable<Product>());


            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 2;

            ProductListViewModel result = controller.List(null, 2).ViewData.Model as ProductListViewModel;

            Product[] products = result.Products.ToArray();

            Assert.True(products.Length == 2);
            Assert.Equal("p3", products[0].Name);
            Assert.Equal("p4", products[1].Name);
        }

        [Fact]
        public void Can_FilterProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "p1", Category = "cat1"},
                new Product() {ProductID = 2, Name = "p2", Category = "cat2"},
                new Product() {ProductID = 3, Name = "p3", Category = "cat1"},
                new Product() {ProductID = 4, Name = "p4", Category = "cat2"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);

            ProductListViewModel result = controller.List("cat2", 1).Model as ProductListViewModel;

            Product[] products = result.Products.ToArray();

            Assert.Equal(2, products.Length);
            Assert.True(products[0].Category == "cat2" && products[0].ProductID == 2);
            Assert.True(products[1].Category == "cat2" && 4 == products[1].ProductID);
        }

        [Fact]
        public void Can_SpecifficProductCount()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "p1", Category = "cat1"},
                new Product() {ProductID = 2, Name = "p2", Category = "cat2"},
                new Product() {ProductID = 3, Name = "p3", Category = "cat1"},
                new Product() {ProductID = 4, Name = "p4", Category = "cat2"},
                new Product() {ProductID = 5, Name = "p5", Category = "cat2"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);

            ProductListViewModel result1 = controller.List("cat1", 1).Model as ProductListViewModel;
            ProductListViewModel result2 = controller.List("cat2", 1).Model as ProductListViewModel;
            ProductListViewModel resultAll = controller.List(null, 1).Model as ProductListViewModel;

            Assert.Equal(2, result1.PagingInfo.TotalItems);
            Assert.Equal(3, result2.PagingInfo.TotalItems);
            Assert.Equal(5, resultAll.PagingInfo.TotalItems);
        }
    }
}