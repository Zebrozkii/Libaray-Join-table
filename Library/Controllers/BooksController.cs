using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        [HttpGet("/books")]
        public ActionResult Index()
        {
            List<Book> allItems = Book.GetAll();
            return View(allItems);
        }

        [HttpGet("/books/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/books")]
        public ActionResult Create(string itemDescription)
        {
            Book newBook = new Book(itemDescription);
            newBook.Save();
            List<Book> allItems = Book.GetAll();
            return View("Index", allItems);
        }

        [HttpGet("/books/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Book selectedBooks = Book.Find(id);
            List<Author> AuthorCategories = selectedBooks.GetAuthors();
            List<Author> allAuthors = Author.GetAll();
            model.Add("selectedBook", selectedBooks);
            model.Add("bookAuthors", AuthorCategories);
            model.Add("allAuthors", allAuthors);
            return View(model);
        }

        [HttpPost("/books/{bookId}/authors/new")]
        public ActionResult AddAuthor(int bookId, int authorId)
        {
            Book item = Book.Find(bookId);
            Author author = Author.Find(authorId);
            item.AddAuthor(author);
            return RedirectToAction("Show",  new { id = bookId });
        }

        [HttpPost("/book/delete")]
        public ActionResult DeleteAll()
        {
            Book.ClearAll();
            return View();
        }

        [HttpGet("/books/{bookId}/edit")]
        public ActionResult Edit(int authorId, int bookId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Book book = Book.Find(bookId);
            model.Add("book", book);
            return View(model);
        }

        [HttpPost("/books/{bookId}")]
        public ActionResult Update(int authorId, int bookId, string newTitle)
        {
            Book book = Book.Find(bookId);
            book.Edit(newTitle);
            Dictionary<string, object> model = new Dictionary<string, object>();
            List<Author> bookAuthor = book.GetAuthors();
            List<Author> allAuthors = Author.GetAll();
            model.Add("selectedBook", book);
            model.Add("bookAuthors", bookAuthor);
            model.Add("allAuthors", allAuthors);
            return View("Show", model);
        }

        [HttpGet("/books/{bookId}/delete")]
        public ActionResult Delete(int authorId, int bookId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Book book = Book.Find(bookId);
            book.Delete();
            model.Add("book", book);
            return View(model);
        }

        [HttpPost("/books/deleted")]
        public ActionResult DeleteItem(int bookId)
        {
            Book book = Book.Find(bookId);
            book.Delete();
            return RedirectToAction("Index");
        }
    }
}
