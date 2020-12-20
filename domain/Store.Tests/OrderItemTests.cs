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
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, count, 0.0m));
        }
        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            int count = -1;
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, count, 0.0m));
        }
        [Fact]
        public void OrderItem_WithPositiveCount_ThrowsArgumentOutOfRangeException()
        {
            var item = new OrderItem(1, 1, 1.0m);
            Assert.Equal(1, item.BookId);
            Assert.Equal(1, item.Count);
            Assert.Equal(1.0m, item.Price);
        }
    }
}
