using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {

    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/categories/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult Create(string categoryName)
    {
      Category newCategory = new Category(categoryName);
      newCategory.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/categories/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Item> categoryItems = selectedCategory.GetItems();
      model.Add("category", selectedCategory);
      model.Add("items", categoryItems);
      return View(model);
    }

    [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(string itemDescription, int categoryId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemDescription, categoryId);
      List<Item> categoryItems = foundCategory.GetItems();
      newItem.Save();
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Show", model);
    }

    [HttpPost("/categories/{categoryId}")]
      public ActionResult DeleteItem(int categoryId, int itemId)
      {
        Item item = Item.Find(itemId);           
        item.Delete();
        return RedirectToAction("Show");
      }    
  }
}
