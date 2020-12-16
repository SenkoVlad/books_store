using System;

namespace Store
{
    public class Book
    {
        public int Id { get; }
        public string Title { get; }
        Book(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
