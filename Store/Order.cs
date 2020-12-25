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
        public IReadOnlyCollection<OrderItem> Items => items;
        public int TotalCount => items.Sum(item => item.Count);
        public decimal TotalPrice => items.Sum(item => item.Price * item.Count);
        public OrderState State { get; private set; }
        public string CellPhone { get; set; }
        public Order(int id, OrderState state, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Id = id;
            State = state;
            this.items = new List<OrderItem>(items); 
        }
        public OrderItem GetItem(int bookId)
        {
            int index = items.FindIndex(elem => elem.BookId == bookId);
            if(index == -1)
                throw new InvalidOperationException("Cart doesn`t contain this item");

            return items[index];
        }

        public void AddOrUpdateItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException();

            var item = items.SingleOrDefault(elem => elem.BookId == book.Id);

            int index = items.FindIndex(elem => elem.BookId == book.Id);

            if(index == -1)
                items.Add(new OrderItem(book.Id, count, book.Price));
            else
                items[index].Count += count;
        }
        public void RemoveItem(int bookId)
        {
            int index = items.FindIndex(elem => elem.BookId == bookId);
            
            if(index == -1)
                throw new InvalidOperationException("Cart doesn`t contain this item");

             items.RemoveAll(x => x.BookId == bookId);
        }
    }
}
