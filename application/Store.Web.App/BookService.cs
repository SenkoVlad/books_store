using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Web.App
{
    public class BookService
    {
        private IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public IReadOnlyCollection<BookModel> GetAllByQuery(string query)
        {
            var books = Book.isIsbn(query) 
                        ? bookRepository.GetAllByIsbn(query) 
                        : bookRepository.GetAllByTitleOrAuthor(query);


            return books.Select(Map).ToArray();
        }

        public BookModel GetById(int id)
        {
            var book = bookRepository.GetById(id);
            return Map(book);
        }

        private BookModel Map(Book book)
        {
            return new BookModel
            {
                Id = book.Id,
                Isbn = book.Isbn,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Price = book.Price
            };
        }
    }
}
