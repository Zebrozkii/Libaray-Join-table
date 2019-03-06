using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Tests
{
    [TestClass]
    public class AuthorTest : IDisposable
    {

        public AuthorTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
        }

        public void Dispose()
        {
            Author.ClearAll();
            Book.ClearAll();
        }

        // [TestMethod]
        // public void AuthorConstructor_CreatesInstanceOfAuthor_Author()
        // {
        //     Author newAuthor = new Author("test category");
        //     Assert.AreEqual(typeof(Author), newAuthor.GetType());
        // }

        // [TestMethod]
        // public void GetName_ReturnsName_String()
        // {
        //     //Arrange
        //     string name = "Test Author";
        //     Author newAuthor = new Author(name);

        //     //Act
        //     string result = newAuthor.GetName();

        //     //Assert
        //     Assert.AreEqual(name, result);
        // }

        // // [TestMethod]
        // // public void GetId_ReturnsAuthorId_Int()
        // // {
        // //   //Arrange
        // //   string name = "Test Author";
        // //   Author newAuthor = new Author(name);
        // //
        // //   //Act
        // //   int result = newAuthor.GetId();
        // //
        // //   //Assert
        // //   Assert.AreEqual(1, result);
        // // }

        // [TestMethod]
        // public void GetAll_ReturnsAllAuthorObjects_AuthorList()
        // {
        //     //Arrange
        //     string name01 = "Work";
        //     string name02 = "School";
        //     Author newAuthor1 = new Author(name01);
        //     newAuthor1.Save();
        //     Author newAuthor2 = new Author(name02);
        //     newAuthor2.Save();
        //     List<Author> newList = new List<Author> { newAuthor1, newAuthor2 };

        //     //Act
        //     List<Author> result = Author.GetAll();

        //     //Assert
        //     CollectionAssert.AreEqual(newList, result);
        // }

        // [TestMethod]
        // public void Find_ReturnsAuthorInDatabase_Author()
        // {
        //     //Arrange
        //     Author testAuthor = new Author("Household chores");
        //     testAuthor.Save();

        //     //Act
        //     Author foundAuthor = Author.Find(testAuthor.GetId());

        //     //Assert
        //     Assert.AreEqual(testAuthor, foundAuthor);
        // }

        // [TestMethod]
        // public void GetBooks_ReturnsEmptyBookList_BookList()
        // {
        //     //Arrange
        //     string name = "Work";
        //     Author newAuthor = new Author(name);
        //     List<Book> newList = new List<Book> { };

        //     //Act
        //     List<Book> result = newAuthor.GetBooks();

        //     //Assert
        //     CollectionAssert.AreEqual(newList, result);
        // }

        // [TestMethod]
        // public void GetAll_CategoriesEmptyAtFirst_List()
        // {
        //     //Arrange, Act
        //     int result = Author.GetAll().Count;

        //     //Assert
        //     Assert.AreEqual(0, result);
        // }

        // [TestMethod]
        // public void Equals_ReturnsTrueIfNamesAreTheSame_Author()
        // {
        //     //Arrange, Act
        //     Author firstAuthor = new Author("Household chores");
        //     Author secondAuthor = new Author("Household chores");

        //     //Assert
        //     Assert.AreEqual(firstAuthor, secondAuthor);
        // }

        // [TestMethod]
        // public void Save_SavesAuthorToDatabase_AuthorList()
        // {
        //     //Arrange
        //     Author testAuthor = new Author("Household chores");
        //     testAuthor.Save();

        //     //Act
        //     List<Author> result = Author.GetAll();
        //     List<Author> testList = new List<Author> { testAuthor };

        //     //Assert
        //     CollectionAssert.AreEqual(testList, result);
        // }

        // [TestMethod]
        // public void Save_DatabaseAssignsIdToAuthor_Id()
        // {
        //     //Arrange
        //     Author testAuthor = new Author("Household chores");
        //     testAuthor.Save();

        //     //Act
        //     Author savedAuthor = Author.GetAll()[0];

        //     int result = savedAuthor.GetId();
        //     int testId = testAuthor.GetId();

        //     //Assert
        //     Assert.AreEqual(testId, result);
        // }

        // [TestMethod]
        // public void GetBooks_RetrievesAllBooksWithAuthor_BookList()
        // {
        //     //Arrange, Act
        //     Author testAuthor = new Author("Household chores");
        //     testAuthor.Save();
        //     Book firstBook = new Book("Mow the lawn", testAuthor.GetId());
        //     firstBook.Save();
        //     Book secondBook = new Book("Do the dishes", testAuthor.GetId());
        //     secondBook.Save();
        //     List<Book> testBookList = new List<Book> { firstBook, secondBook };
        //     List<Book> resultBookList = testAuthor.GetBooks();

        //     //Assert
        //     CollectionAssert.AreEqual(testBookList, resultBookList);
        // }

        [TestMethod]
        public void Delete_DeletesAuthorAssociationsFromDatabase_AuthorList()
        {
            Book testBook = new Book("Mow the lawn");
            testBook.Save();
            string testName = "Home stuff";
            Author testAuthor = new Author(testName);
            testAuthor.Save();
            testAuthor.AddBook(testBook);
            testAuthor.Delete();
            List<Author> resultBookCategories = testBook.GetAuthors();
            List<Author> testBookCategories = new List<Author> {};
            CollectionAssert.AreEqual(testBookCategories, resultBookCategories);
        }

        [TestMethod]
        public void Test_AddBook_AddsBookToAuthor()
        {
            Author testAuthor = new Author("Household chores");
            testAuthor.Save();
            Book testBook = new Book("Mow the lawn");
            testBook.Save();
            Book testBook2 = new Book("Water the garden");
            testBook2.Save();
            testAuthor.AddBook(testBook);
            testAuthor.AddBook(testBook2);
            List<Book> result = testAuthor.GetBooks();
            List<Book> testList = new List<Book>{testBook, testBook2};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetBooks_ReturnAllAuthorBook_BookList()
        {
            Author testAuthor = new Author("Houshold chores");
            testAuthor.Save();
            Book testBook1 = new Book("Mow the lawn");
            testBook1.Save();
            Book testBook2 = new Book("Buy plane ticket");
            testBook2.Save();

            testAuthor.AddBook(testBook1);
            List<Book> savedBooks = testAuthor.GetBooks();
            List<Book> testList = new List<Book> {testBook1};
            CollectionAssert.AreEqual(testList, savedBooks);
        }
    }
}
