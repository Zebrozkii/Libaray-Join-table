using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Item
    {

        private string _description;
        private static List<Item> _instances = new List<Item> {};

        public Item(string description)
        {
            _description = description;
            _instances.Add(this);
        }

        public string GetDescription()
        {
            return _description;
        }

        public void SetDescription(string newDescription)
        {
            _description = newDescription;
        }

        public static List<Item> GetAll()
        {
            return _instances;
        }

        public static void ClearAll()
        {
            _instances.Clear();
        }

        // public static void userAnswerAddOrView(string userAnswer)
        // {
        //     if (userAnswer == "add")
        //     {
        //         Console.WriteLine("Please enter the description for the new item:");
        //         string userItemInput = Console.ReadLine();
        //         Item newItem = new Item(userItemInput);
        //         string result = newItem.GetDescription();
        //         Console.WriteLine(result + " has been added to your list.");
        //         Main();
        //     }
        //     else if (userAnswer == "view")
        //     {
        //         Console.WriteLine("YOUR GROCERY LIST:");
        //         foreach (Item item in Item.GetAll())
        //         {
        //             Console.WriteLine("---------------------");
        //             Console.WriteLine(item.GetDescription());
                    
        //         }
        //         Main();
        //     }
        //     else
        //     {
        //         Console.WriteLine("Goodbye!");
        //     }
        // }

        // public static void Main()
        // {
        //     Console.WriteLine("Welcome to the To Do List!");
        //     Console.WriteLine("Would you like to add an item to your list or view your list? (add/view)");
        //     string addOrView = Console.ReadLine();
        //     userAnswerAddOrView(addOrView);
        // }
    }
}