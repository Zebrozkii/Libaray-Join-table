using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class AuthorController : Controller
    {

        [HttpGet("/authors")]
        public ActionResult Index()
        {
            List<Author> allCategories = Author.GetAll();
            return View(allCategories);
        }

        [HttpGet("/authors/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/authors")]
        public ActionResult Create(string authorName)
        {
            Author newAuthor = new Author(authorName);
            newAuthor.Save();
            List<Author> allCategories = Author.GetAll();
            return View("Index", allCategories);
        }

        [HttpGet("/authors/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Author selectedAuthor = Author.Find(id);
            List<Book> authorBook = selectedAuthor.GetBooks();
            List<Book> allItems = Book.GetAll();
            model.Add("author", selectedAuthor);
            // Console.WriteLine("category {0} {1}", selectedCategory.GetId(), selectedCategory.GetName());
            model.Add("authorBook", authorBook);
            // Console.WriteLine("items {0}", categoryItems.Count);
            model.Add("allItems", allItems);
            return View(model);
        }

        [HttpPost("/authors/{authorId}/books/new")]
        public ActionResult AddItem(int authorId, int bookId)
        {
            Author author = Author.Find(authorId);
            Book book = Book.Find(bookId);
            author.AddBook(book);
            return RedirectToAction("Show", new { id = authorId });
        }
    }
}
