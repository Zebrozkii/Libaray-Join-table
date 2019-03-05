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

    [HttpGet("/categories/{id}/sorted_due_dates")]
    public ActionResult SortByDueDate(int id)
    {
      return RedirectToAction("Show", new { id = id, due_date_sort = true, name_sort = false});
    }

    [HttpGet("/categories/{id}/sorted_names")]
    public ActionResult SortByName(int id)
    {
      return RedirectToAction("Show", new { id = id, due_date_sort = false, name_sort = true});
    }

    [HttpGet("/categories/{id}")]
    public ActionResult Show(int id, bool due_date_sort = false, bool name_sort = false)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Item> categoryItems = selectedCategory.GetItems(due_date_sort, name_sort);
      model.Add("category", selectedCategory);
     // Console.WriteLine("category {0} {1}", selectedCategory.GetId(), selectedCategory.GetName());
      model.Add("items", categoryItems);
     // Console.WriteLine("items {0}", categoryItems.Count);
      return View(model);
    }

    [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(string itemDescription, DateTime dueDate, int categoryId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemDescription, dueDate, categoryId);
      newItem.Save();
      foundCategory.AddItem(newItem);
      List<Item> categoryItems = foundCategory.GetItems();     
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Show", model);
    }

    [HttpPost("/categories/{categoryId}")]
    public ActionResult DeleteItem(int categoryId, int itemId)
    {
      Item item = Item.Find(itemId);           
      item.Delete();
      return RedirectToAction("Index");
    }
  }
}
