using System;
using Xunit;

namespace Store.Tests
{
    public class BookTests
    {
        [Fact]
        public void IsIsbn_WithNull_ReturnFalse()
        {
            bool actual = Book.isIsbn(null);
            Assert.False(actual);
        }

        [Fact]
        public void IsIsbn_WithBlankString_ReturnFalse()
        {
            bool actual = Book.isIsbn("    ");
            Assert.False(actual);
        }

        [Fact]
        public void IsIsbn_WithInvalidIsbn_ReturnFalse()
        {
            bool actual = Book.isIsbn("ISBN 123");
            Assert.False(actual);
        }

        [Fact]
        public void IsIsbn_WithIsbn10_ReturnTrue()
        {
            bool actual = Book.isIsbn("ISBN 123-345-5-234");
            Assert.True(actual);
        }
        [Fact]
        public void IsIsbn_WithIsbn13_ReturnTrue()
        {
            bool actual = Book.isIsbn("ISBN 123-345-2345-234");
            Assert.True(actual);
        }
        [Fact]
        public void IsIsbn_WithTrashStart_ReturnTrue()
        {
            bool actual = Book.isIsbn("xxxISBN 123-345-2345-234 435 xx");
            Assert.False(actual);
        }
    }
}
