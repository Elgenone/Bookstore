using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Models.Reposatories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        public IBookStoreRepository<Book> BookRepository { get; }
        public IBookStoreRepository<Author> AuthorRepository { get; }
        public IHostingEnvironment Hosting { get; }

        public BookController(IBookStoreRepository<Book> bookRepository,
            IBookStoreRepository<Author> authorRepository,
            IHostingEnvironment hosting)
        {
            BookRepository = bookRepository;
            AuthorRepository = authorRepository;
            Hosting = hosting;
        }
        // GET: Book
        public ActionResult Index()
        {
            var books = BookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = BookRepository.Find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View(GetAllAuthors());
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if the UploadFile return null make the fileName empty
                string fileName = UploadFile(model.File) ?? string.Empty;
               

                try
                {
                    // TODO: Add insert logic here
                    if (model.AuthorId == -1)
                    {
                        ViewBag.message = "Please select an Author from the List";
                        
                        return View(GetAllAuthors());
                    }
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = AuthorRepository.Find(model.AuthorId),
                        ImageUrl = fileName
                    };
                    BookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "Please Fill all required fildes");
            return View(GetAllAuthors());
           
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = BookRepository.Find(id);
            var authorId = book.Author
                           == null ? book.Author.Id = 0 : book.Author.Id;
            var model = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = FillSelectedList(),
                ImageUrl = book.ImageUrl
            };
            return View(model);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel model)
        {
            try
            {
               
                //if the UploadFile return null make the fileName empty
                string fileName = UploadFile(model.File,model.ImageUrl);

                Book book = new Book
                {
                    Id=model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    Author = AuthorRepository.Find(model.AuthorId),
                    ImageUrl=fileName
                };
                BookRepository.Update(model.BookId,book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = BookRepository.Find(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                BookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //return authors + the first select option
        List<Author> FillSelectedList()
        {
           var authors = AuthorRepository.List().ToList();
            authors.Insert(0, 
                new Author { Id = -1, Name = "--- Please Select an Author ---" });
            return authors;
        }

        //return authors
        BookAuthorViewModel GetAllAuthors()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectedList()
            };
            return model;
        }

        //return the path of image
        string UploadFile(IFormFile File)
        {
            if (File != null)
            {
                // the path of folder images in the project
                string uplaods = Path.Combine(Hosting.WebRootPath/*WWWroot Path*/,
                    "images" /*images folder in wwwroot*/);


                // the full path of the image in the project 
                string fullPath = Path.Combine(uplaods, File.FileName/*the name of the image.extention(jpg,png,...etc)*/);

                //copy the file to this path in file stream
                File.CopyTo(new FileStream(fullPath, FileMode.Create));

                return File.FileName;
            }
            return null;
        }

        //return the path of image
        string UploadFile(IFormFile File,string ImageUrl)
        {
            if (File != null)
            {
                // the path of folder images in the project
                string uplaods = Path.Combine(Hosting.WebRootPath/*WWWroot Path*/,
                    "images" /*images folder in wwwroot*/);

               

                // the full path of the image in the project 
                string newPath = Path.Combine(uplaods, File.FileName);

                //delete the old image
                string oldpath = Path.Combine(uplaods, ImageUrl);

                // if the old image is the same the new image don't do anything
                if (newPath != oldpath)
                {
                    System.IO.File.Delete(oldpath);

                    //copy the file to this path in file stream
                    File.CopyTo(new FileStream(newPath, FileMode.Create));
                }
                return File.FileName;
            }
            return ImageUrl;
        }

        public ActionResult Search(string term)
        {
            var result = BookRepository.Search(term);
            return View("index",result);
        }
    }
}