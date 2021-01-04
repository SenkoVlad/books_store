using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Data.EF
{
    class OrderRepository : IOrderRepository
    {
        private DbContextFactory dbContextFactory;

        public OrderRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
        public Order Create()
        {
            var dbContex = dbContextFactory.Create(typeof(OrderRepository));

            var dto = Order.DtoFactory.Create();
            dbContex.Orders.Add(dto);
            dbContex.SaveChanges();

            return Order.Mapper.Map(dto);
        }

        public Order GetById(int id)
        {
            var dbContex = dbContextFactory.Create(typeof(OrderRepository));

            var dto = dbContex.Orders
                              .Include(order => order.Items)
                              .Single(order => order.Id == id);

            return Order.Mapper.Map(dto);
        }

        public void Update(Order order)
        {
            var dbContex = dbContextFactory.Create(typeof(OrderRepository));
            dbContex.SaveChanges();
        }
    }
}
