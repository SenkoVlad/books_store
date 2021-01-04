using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Data.EF
{
    class BookRepository : IBookRepository
    {
        private DbContextFactory dbContextFactory;

        public BookRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var dbContex = dbContextFactory.Create(typeof(BookRepository));

            return dbContex.Books
                           .Where(book => bookIds.Contains(book.Id))
                           .AsEnumerable()
                           .Select(Book.Mapper.Map)
                           .ToArray();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            var dbContex = dbContextFactory.Create(typeof(BookRepository));

            if(Book.TryFormatIsbn(isbn, out string formattedIsbn))
            {
                return dbContex.Books
                           .Where(book => book.Isbn == formattedIsbn)
                           .AsEnumerable()
                           .Select(Book.Mapper.Map)
                           .ToArray();
            }

            return new Book[0];
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            var dbContex = dbContextFactory.Create(typeof(BookRepository));

            var paramerts = new SqlParameter("@titleAuthor", titleOrAuthor);
            return dbContex.Books
                           .FromSqlRaw("SELECT * FROM Books WHERE CONTAINS((Author, Title), @titleAuthor)", paramerts)
                           .AsEnumerable()
                           .Select(Book.Mapper.Map)
                           .ToArray();
        }

        public Book GetById(int id)
        {
            var dbContex = dbContextFactory.Create(typeof(BookRepository));

            var dto = dbContex.Books
                           .Single(book => book.Id == id);

            return Book.Mapper.Map(dto);
        }
    }
}
