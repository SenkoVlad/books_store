using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"ISBN 12345-12345", "D. Knuth", "Art of programming"),
            new Book(2, "ISBN 13455-436643", "M. Flower", "Refactoring"),
            new Book(3, "ISBN 35346-26467", "B. Ritchie","C Programming Languege")
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
    }
}
