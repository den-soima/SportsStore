using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_SelectCategory()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(p => p.Products).Returns(new Product[]
            {
                new Product() {ProductID = 1, Category = "cat1"},
                new Product() {ProductID = 2, Category = "cat2"},
                new Product() {ProductID = 3, Category = "cat2"},
                new Product() {ProductID = 4, Category = "cat3"},
                new Product() {ProductID = 5, Category = "cat4"},
                new Product() {ProductID = 6, Category = "cat4"}
            }.AsQueryable<Product>());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            Dictionary<string, int> result =
                ((Dictionary<string, int>) (target.Invoke() as ViewViewComponentResult)?.ViewData.Model);
        

            Assert.True(result.Count == 4);
            Assert.Equal("cat4", result.Keys.ToArray()[3]);
            Assert.True(Enumerable.SequenceEqual(new string[] {"cat1", "cat2", "cat3", "cat4"}, result.Keys));
        }

        [Fact]
        public void Indicates_SelectedCategory()
        {
            string categoryToSelect = "cat2";

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[]
            {
                new Product() {ProductID = 1, Name = "p1", Category = "cat1"},
                new Product() {ProductID = 2, Name = "p2", Category = "cat2"}
            }.AsQueryable<Product>());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext {RouteData = new RouteData()}
            };

            target.RouteData.Values["category"] = categoryToSelect;

            string result = (string) (target.Invoke() as ViewViewComponentResult)?.ViewData["SelectedCategory"];
            Assert.Equal(result, categoryToSelect);
        }
    }
}