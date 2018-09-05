using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_AddLines()
        {
            Cart cart = new Cart();
            
            cart.AddItem(new Product() {ProductID = 1, Name = "p1"}, 2);
            cart.AddItem(new Product() {ProductID = 2, Name = "p2"}, 1);
            cart.AddItem(new Product() {ProductID = 1}, 2);

            Product product = new Product() {ProductID = 3, Name = "p3"};
            
            cart.AddItem(product, 3);
            
            Assert.Equal(3, cart.Lines.Count());
            Assert.Equal(4, cart.Lines.Where(p => p.Product.ProductID == 1).Sum(q => q.Quantity));
            Assert.Equal(product, cart.Lines.ToArray()[2].Product);
        }
        
        [Fact]
        public void Can_RemoveItem()
        {
            Cart cart = new Cart();
            
            cart.AddItem(new Product() {ProductID = 1, Name = "p1"}, 2);
            cart.AddItem(new Product() {ProductID = 2, Name = "p2"}, 1);
            cart.AddItem(new Product() {ProductID = 1}, 2);

            Product product = new Product() {ProductID = 1};
            
            cart.RemoveLine(product);
            
            Assert.Equal(1, cart.Lines.Count());
            Assert.Equal("p2", cart.Lines.ToArray()[0].Product.Name);
        }

        [Fact]
        public void Calculate_CartTotal()
        {
            Cart cart = new Cart();
            
            cart.AddItem(new Product() {ProductID = 1, Price = 10}, 2);
            cart.AddItem(new Product() {ProductID = 2, Price = 30}, 1);

            Assert.Equal(50, cart.ComputeTotalValues);
        }

        [Fact]
        public void Can_ClearContent()
        {
            Cart cart = new Cart();
            
            cart.AddItem(new Product() {ProductID = 1, Price = 10}, 2);
            cart.AddItem(new Product() {ProductID = 2, Price = 30}, 1);
            
            cart.Clear();
            
            Assert.True(!cart.Lines.Any());
        }
            
    }
    
    
    
    
}