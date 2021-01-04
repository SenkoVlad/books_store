using Store.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Store.Tests
{
    public class OrderItemCollectionTests
    {
        [Fact]
        public void GetItem_WithExistingItem_ReturnsItem()
        {
            var order = CreateOrderTest();

            var orderItem = order.Items.Get(1);
            Assert.Equal(3, orderItem.Count);
        }
        [Fact]
        public void GetItem_WithNotExistingItem_ThrowsInvalidOperationException()
        {
            var order = CreateOrderTest();

            Assert.Throws<InvalidOperationException>(() => order.Items.Get(100));
        }
        [Fact]
        public void AddOrUpdateItem_WithExistingItem_UpdatesCount()
        {
            var order = CreateOrderTest();

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(1, 10, 10m);
            });
        }
        [Fact]
        public void RemoveItem_WithExistingItem_ReturnsItem()
        {
            var order = CreateOrderTest();
            order.Items.Remove(2);

            Assert.Throws<InvalidOperationException>(() => order.Items.Get(2));
        }
        [Fact]
        public void RemoveItem_WithNonExistingItem_ReturnsItem()
        {
            var order = CreateOrderTest();

            Assert.Throws<InvalidOperationException>(() => order.Items.Remove(3));
        }

        private static Order CreateOrderTest()
        {
            return new Order(new OrderDto
            {
                Id = 1,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto {BookId = 1, Price = 10m, Count = 3},
                    new OrderItemDto {BookId = 2, Price = 100m, Count = 5},
                }
            });
        }
    }
}
