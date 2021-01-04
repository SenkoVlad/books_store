using Store.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    public class OrderItem
    {
        private readonly OrderItemDto orderItemDto;

        public int BookId => orderItemDto.BookId;
        public int Count
        {
            get => orderItemDto.Count;
            set
            {
                ThrowIfInvalidCount(value);
                orderItemDto.Count = value;
            }
        }
        public decimal Price 
        {
            get => orderItemDto.Price;
            set => orderItemDto.Price = value;
        }
        internal OrderItem(OrderItemDto orderItemDto)
        {
            this.orderItemDto = orderItemDto;
        }

        private static void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count <= 0");
        }
        public static class DtoFactory
        {
            public static OrderItemDto Create(OrderDto orderDto, int bookId, int count, decimal price)
            {
                if(orderDto == null)
                    throw new ArgumentNullException(nameof(orderDto));

                ThrowIfInvalidCount(count);

                return new OrderItemDto
                {
                    BookId = bookId,
                    Price = price,
                    Count = count,
                    Order = orderDto
                };
            }
        }

        public static class Mapper
        {
            public static OrderItem Map(OrderItemDto orderItemDto) => new OrderItem(orderItemDto);
            public static OrderItemDto Map(OrderItem orderItem) => orderItem.orderItemDto;
        }
    }
}
