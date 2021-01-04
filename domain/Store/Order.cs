using Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store
{
    public class Order
    {
        private readonly OrderDto orderDto;
        public int Id => orderDto.Id;

        public string CellPhone 
        {
            get => orderDto.CellPhone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(CellPhone));

                orderDto.CellPhone = value.Trim();
            }
        }
        public OrderDelivery Delivery 
        {
            get
            {
                if (orderDto.DeliveryUniqueCode == null)
                    return null;

                return new OrderDelivery(orderDto.DeliveryUniqueCode, 
                                         orderDto.DeliveryDescription, 
                                         orderDto.DeliveryPrice, 
                                         orderDto.DeliveryParametrs);
            }
            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Delivery));

                orderDto.DeliveryUniqueCode = value.UniqueCode;
                orderDto.DeliveryDescription = value.Description;
                orderDto.DeliveryPrice = value.Price;
                orderDto.DeliveryParametrs = value.Parametrs.ToDictionary(p => p.Key, p => p.Value);
            }
        }
        public OrderPayment Payment 
        {
            get
            {
                if (orderDto.PaymentUniqueCode == null)
                    return null;

                return new OrderPayment(orderDto.PaymentUniqueCode,
                                        orderDto.PaymentDescription,
                                        orderDto.PaymentParametrs);
            }
            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Payment));

                orderDto.PaymentUniqueCode = value.UniqueCode;
                orderDto.PaymentDescription = value.Description;
                orderDto.PaymentParametrs = value.Parametrs.ToDictionary(p => p.Key, p => p.Value);
            }
        }
        public OrderItemCollection Items { get; }
        public int TotalCount => Items.Sum(item => item.Count);
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Count)
                                          + (Delivery?.Price ?? 0m);
        public OrderState State { get; private set; }
        
        public Order(OrderDto orderDto)
        {
            this.orderDto = orderDto;
            this.Items = new OrderItemCollection(orderDto);
        }

        public static class DtoFactory
        {
            public static OrderDto Create() => new OrderDto();
        } 
        public static class Mapper
        {
            public static OrderDto Map(Order order) => order.orderDto;
            public static Order Map(OrderDto orderDto) => new Order(orderDto);
        }
    }
}
