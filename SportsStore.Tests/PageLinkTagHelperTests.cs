using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Controllers;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void Can_GeneratePageLinks()
        {
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)
            {
                PageModel = new PagingInfo()
                {
                    CurrentPage = 2,
                    ItemsPerPage = 10,
                    TotalItems = 28
                },
                PageAction = "Test"
            };
            TagHelperContext ctx =
                new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
            var content = new Mock<TagHelperContent>();

            TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(),
                (cahe, encoder) => Task.FromResult(content.Object));

            helper.Process(ctx, output);

            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
                         + @"<a href=""Test/Page2"">2</a>"
                         + @"<a href=""Test/Page3"">3</a>", output.Content.GetContent());
        }

        [Fact]
        public void Can_SendPaginationViewModel()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(v => v.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "p1"},
                new Product() {ProductID = 2, Name = "p2"},
                new Product() {ProductID = 3, Name = "p3"},
                new Product() {ProductID = 4, Name = "p4"},
                new Product() {ProductID = 5, Name = "p5"},
                new Product() {ProductID = 6, Name = "p6"},
                new Product() {ProductID = 7, Name = "p7"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object) {PageSize = 3};

            ProductListViewModel result = controller.List(2).ViewData.Model as ProductListViewModel;

            PagingInfo info = result.PagingInfo;

            Assert.Equal(2, info.CurrentPage);
            Assert.Equal(3, info.ItemsPerPage);
            Assert.Equal(7, info.TotalItems);
            Assert.Equal(3, info.TotalPages);
        }
    }
}