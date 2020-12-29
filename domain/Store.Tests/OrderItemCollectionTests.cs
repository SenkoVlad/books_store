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
            var order = new Order(1, OrderState.Created, new[]
            {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            var orderItem = order.Items.Get(1);
            Assert.Equal(3, orderItem.Count);
        }
        [Fact]
        public void GetItem_WithNotExistingItem_ThrowsInvalidOperationException()
        {
            var order = new Order(1, OrderState.Created, new[]
            {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            Assert.Throws<InvalidOperationException>(() => order.Items.Get(100));
        }
        [Fact]
        public void AddOrUpdateItem_WithExistingItem_UpdatesCount()
        {
            var order = new Order(1, OrderState.Created, new[]
{
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            var book = new Book(1, "", "", "", "", 10m);

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(1, 10, 10m);
            });
        }
        [Fact]
        public void RemoveItem_WithExistingItem_ReturnsItem()
        {
            var order = new Order(1, OrderState.Created, new[]
            {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });
            order.Items.Remove(2);

            Assert.Throws<InvalidOperationException>(() => order.Items.Get(2));
        }
        [Fact]
        public void RemoveItem_WithNonExistingItem_ReturnsItem()
        {
            var order = new Order(1, OrderState.Created, new[]
            {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 5, 100m)
            });

            Assert.Throws<InvalidOperationException>(() => order.Items.Remove(3));
        }
    }
}
