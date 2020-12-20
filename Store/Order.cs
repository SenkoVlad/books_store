using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store
{
    public class Order
    {
        public int Id { get; }
        private List<OrderItem> items;
        public IReadOnlyCollection<OrderItem> Items
        {
            get { return items; }
        }
        public int TotalCount
        {
            get { return items.Sum(item => item.Count); }
        }
        public decimal TotalPrice
        {
            get { return items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Id = id;
            this.items = new List<OrderItem>(items); 
        }
        public void AddItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException();

            var item = items.SingleOrDefault(elem => elem.BookId == book.Id);

            if(item == null)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items.Remove(item);
                items.Add(new OrderItem(book.Id, count + item.Count, book.Price));
            }
        }
    }
}
