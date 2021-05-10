using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Reposatories
{
    public class BookRepository : IBookStoreRepository<Book>
    {
        private readonly List<Book> books;
        public BookRepository()
        {
            books = new List<Book>
            {
                new Book
                {
                    Id=1,Title="C# how to brogram",Description="no descreption",
                    Author =new Author{Id=2},ImageUrl="images.jpeg"
                },
                   new Book
                {
                    Id=2,Title="Python how to brogram",Description="nothing",Author=new Author(),ImageUrl="book.jpeg"
                },
                      new Book
                {
                    Id=3,Title="C++ how to brogram",Description="no data",Author=new Author()
                }
            };
        }
        public void Add(Book book)
        {
            book.Id = books.Max(b => b.Id) + 1;
            books.Add(book);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            _ = books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public IList<Book> Search(string term)
        {
            IEnumerable<Book> result = books.Where(b => b.Title.Contains(term)
            || b.Description.Contains(term) || b.Author.Name.Contains(term));
            return result.ToList();
        }

        public void Update(int id,Book newBook)
        {
            var book = Find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageUrl = newBook.ImageUrl;
        }
    }
}
