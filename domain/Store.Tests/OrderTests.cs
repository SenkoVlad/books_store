using Store.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Store.Tests
{
    public class OrderTests
    {
        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero()
        {
            var order = CreateEmptyOrderTest();
            Assert.Equal(0, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero()
        {
            var order = CreateEmptyOrderTest();
            Assert.Equal(0m, order.TotalPrice);
        }
        [Fact]
        public void TotalCount_WithNotEmptyItems_CalculatesTotalCount()
        {
            var order = CreateOrderTest();
            Assert.Equal(3 + 5, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithNotEmptyItems_CalculatesTotalPrice()
        {
            var order = CreateOrderTest();
            Assert.Equal(3 * 10m + 5 * 100m, order.TotalPrice);
        }

        public static Order CreateEmptyOrderTest()
        {
            return new Order(new OrderDto
            {
                Id = 1,
                Items = new OrderItemDto[0]
            });
        }

        public static Order CreateOrderTest()
        {
            return new Order(new OrderDto
            {
                Id = 1,
                Items = new[]
                {
                    new OrderItemDto {BookId = 1, Price = 10m, Count = 3},
                    new OrderItemDto {BookId = 2, Price = 100m, Count = 5},
                }
            });
        }
    }
}
     