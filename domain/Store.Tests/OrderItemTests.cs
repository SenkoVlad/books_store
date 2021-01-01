using Store.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Store.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowsArgumentOutOfRangeException()
        {
            int count = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => OrderItem.DtoFactory.Create(new OrderDto(), 1, count, 0.0m));
        }
        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            int count = -1;
            Assert.Throws<ArgumentOutOfRangeException>(() => OrderItem.DtoFactory.Create(new OrderDto(), 1, count, 0.0m));
        }
        [Fact]
        public void OrderItem_WithPositiveCount_ThrowsArgumentOutOfRangeException()
        {
            var item = OrderItem.DtoFactory.Create(new OrderDto(), 1, 1, 1.0m);
            Assert.Equal(1, item.BookId);
            Assert.Equal(1, item.Count);
            Assert.Equal(1.0m, item.Price);
        }
        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOfRangeException()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 0, 5, 0.0m);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);
            Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.Count = -1);
        }
        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOfRangeException()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 0, 5, 0.0m);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);

            Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.Count = 0);
        }
        [Fact]
        public void Count_WithPositiveValue_SetValue()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 0, 5, 0.0m);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);

            orderItem.Count = 10;
            Assert.Equal(10, orderItem.Count);
        }  
    }
}
