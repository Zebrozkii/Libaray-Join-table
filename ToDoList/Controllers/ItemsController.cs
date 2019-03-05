using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {

        [HttpGet("/categories/{categoryId}/items/new")]
        public ActionResult New(int categoryId)
        {
            Category category = Category.Find(categoryId);
            return View(category);
        }

        [HttpGet("/categories/{categoryId}/items/{itemId}")]
        public ActionResult Show(int categoryId, int itemId)
        {
            Item item = Item.Find(itemId);
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category category = Category.Find(categoryId);
            model.Add("item", item);
            model.Add("category", category);
            return View(model);
        }

        [HttpPost("/items/delete")]
        public ActionResult DeleteAll()
        {
            Item.ClearAll();
            return View();
        }

        [HttpGet("/categories/{categoryId}/items/{itemId}/edit")]
        public ActionResult Edit(int categoryId, int itemId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category category = Category.Find(categoryId);
            model.Add("category", category);
            Item item = Item.Find(itemId);
            model.Add("item", item);
            return View(model);
        }

        [HttpPost("/categories/{categoryId}/items/{itemId}")]
        public ActionResult Update(int categoryId, int itemId, string newDescription, DateTime newDueDate)
        {
            Item item = Item.Find(itemId);
            item.Edit(newDescription, newDueDate);
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category category = Category.Find(categoryId);
            model.Add("category", category);
            model.Add("item", item);
            return View("Show", model);
        }

        [HttpGet("/categories/{categoryId}/items/{itemId}/delete")]
        public ActionResult Delete(int categoryId, int itemId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category category = Category.Find(categoryId);
            Item item = Item.Find(itemId);
            model.Add("category", category);
            model.Add("item", item);
            return View(model);
        }
    }
}
