using Store.Data;
using System;
using System.Text.RegularExpressions;

namespace Store
{
    public class Book
    {
        private readonly BookDto bookDto;
        public int Id => bookDto.Id;
        public string Title
        {
            get => bookDto.Title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(Title));

                bookDto.Title = value?.Trim();
            }
        }
        public string Isbn
        {
            get => bookDto.Isbn;
            set
            {
                if (TryFormatIsbn(value, out string formattedIsbn))
                    bookDto.Isbn = formattedIsbn;
                else
                    throw new ArgumentNullException(nameof(Isbn));
            }
        }
        public string Author
        {
            get => bookDto.Author;
            set => bookDto.Author = value?.Trim();
        }
        public decimal Price
        {
            get => bookDto.Price;
            set => bookDto.Price = value;
        }
        public string Description 
        {
            get => bookDto.Description;
            set => bookDto.Description = value?.Trim();
        } 

        internal Book(BookDto bookDto)
        {
            this.bookDto = bookDto;
        }

        public static bool TryFormatIsbn(string isbn, out string formattedIsbn)
        {
            if(isbn == null)
            {
                formattedIsbn = null;
                return false;
            }

            formattedIsbn = isbn.Replace("-", "")
             .Replace(" ", "")
             .ToUpper();

            return Regex.IsMatch(formattedIsbn, "^ISBN\\d{10}(\\d{3})?$");

        }
        public static bool isIsbn(string isbn) =>
            TryFormatIsbn(isbn, out _);

        public static class DtoFactory 
        { 
            public static BookDto Create(string isbn, string title, string description, string author, decimal price)
            {
                if (TryFormatIsbn(isbn, out string formattedIsbn))
                    isbn = formattedIsbn;
                else
                    throw new ArgumentNullException(nameof(isbn));

                if (string.IsNullOrWhiteSpace(title))
                    throw new ArgumentNullException(nameof(title));

                return new BookDto
                {
                    Isbn = isbn,
                    Title = title.Trim(),
                    Author = author?.Trim(),
                    Description = description?.Trim(),
                    Price = price
                };
            }
        }

        public static class Mapper
        {
            public static Book Map(BookDto bookDto) => new Book(bookDto);
            public static BookDto Map(Book book) => book.bookDto;
        }
    }
}
