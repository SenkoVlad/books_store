using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store
{
    public class Order
    {
        public int Id { get; }
        public OrderItemCollection Items { get; }
        public int TotalCount => Items.Sum(item => item.Count);
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Count)
                                          + (Delivery?.Amount ?? 0m);
        public OrderState State { get; private set; }
        public string CellPhone { get; set; }
        public OrderDelivery Delivery { get; set; }
        public OrderPayment Payment { get; set; }

        public Order(int id, OrderState state, IEnumerable<OrderItem> items)
        {
            Id = id;
            State = state;
            this.Items = new OrderItemCollection(items); 
        }
    }
}
