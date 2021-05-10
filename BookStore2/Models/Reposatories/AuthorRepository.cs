using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Reposatories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {
        private readonly List<Author> authors;
        public AuthorRepository()
        {
            authors = new List<Author>
                {
                    new Author
                    {
                        Id=1,Name="mahmoued"
                    },
                    new Author
                    {
                        Id=2,Name="ahmed"
                    },
                    new Author
                    {
                        Id=3,Name="mostafa"
                    },
                };
        }   
        public void Add(Author author)
        {
            author.Id = authors.Max(a => a.Id) + 1;

            authors.Add(author);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            authors.Remove(author);
        }

        public Author Find(int id)
        {
            var author = authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return authors;
        }

        public IList<Author> Search(string term)
        {
            var result = authors.Where(a => a.Name.Contains(term));
            return result.ToList();
        }

        public void Update(int id, Author newAuthor)
        {
            var author = Find(id);
            author.Name = newAuthor.Name;
        }
    }
}
