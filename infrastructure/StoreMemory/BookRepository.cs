using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"ISBN 12345-12345", "D. Knuth", "Art of programming", "The most popular book about clear code", 12.2m),
            new Book(2, "ISBN 13455-436643", "M. Flower", "Refactoring", "It is very interesting book about refactoring", 21.22m),
            new Book(3, "ISBN 35346-26467", "B. Ritchie","C Programming Languege", "This book about C", 14.24m)
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn)
                        .ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string querty)
        {
            return books.Where(book => book.Title.Contains(querty) || book.Author.Contains(querty))
                        .ToArray();
        }

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}
