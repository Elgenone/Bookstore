using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Reposatories
{
    public class BookDbRepository : IBookStoreRepository<Book>
    {
        BookStoreDbContext db;
        public BookDbRepository(BookStoreDbContext db)
        {
            this.db = db;
        }
        public void Add(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);
            _ = db.Books.Remove(book);
            db.SaveChanges();

        }

        public Book Find(int id)
        {
            var book = db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return db.Books.Include(a=>a.Author).ToList();
        }

        public void Update(int id, Book newBook)
        {
            db.Update(newBook);
            db.SaveChanges();

        }
        public IList<Book> Search(string term)
        {

            var result = db.Books.Include(a => a.Author).Where(b => b.Title.Contains(term) 
            || b.Description.Contains(term) || b.Author.Name.Contains(term));
            return result.ToList();
        }
    }
}