using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Tests
{
    [TestClass]
    public class BookTest : IDisposable
    {

        public void Dispose()
        {
            Book.ClearAll();
            Author.ClearAll();
        }

        public BookTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
        }

        [TestMethod]
        public void BookConstructor_CreatesInstanceOfBook_Book()
        {
            Book newBook = new Book("test");
            Assert.AreEqual(typeof(Book), newBook.GetType());
        }

        [TestMethod]
        public void GetDescription_ReturnsDescription_String()
        {
            //Arrange
            string description = "Walk the dog.";
            Book newBook = new Book(description);

            //Act
            string result = newBook.GetBookTitle();

            //Assert
            Assert.AreEqual(description, result);
        }

        [TestMethod]
        public void SetDescription_SetDescription_String()
        {
            //Arrange
            string description = "Walk the dog.";
            Book newBook = new Book(description);

            //Act
            string updatedDescription = "Do the dishes";
            newBook.SetBookTitle(updatedDescription);
            string result = newBook.GetBookTitle();

            //Assert
            Assert.AreEqual(updatedDescription, result);
        }

        [TestMethod]
        public void GetAll_ReturnsEmptyList_BookList()
        {
            //Arrange
            List<Book> newList = new List<Book> { };

            //Act
            List<Book> result = Book.GetAll();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void GetAll_ReturnsBooks_BookList()
        {
            //Arrange
            string description01 = "Walk the dog";
            string description02 = "Wash the dishes";
            Book newBook1 = new Book(description01);
            newBook1.Save();
            Book newBook2 = new Book(description02);
            newBook2.Save();
            List<Book> newList = new List<Book> { newBook1, newBook2 };

            //Act
            List<Book> result = Book.GetAll();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Find_ReturnsCorrectBookFromDatabase_Book()
        {
            //Arrange
            Book testBook = new Book("Mow the lawn");
            testBook.Save();

            //Act
            Book foundBook = Book.Find(testBook.GetId());

            //Assert
            Assert.AreEqual(testBook, foundBook);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Book()
        {
            // Arrange, Act
            Book firstBook = new Book("Mow the lawn");
            Book secondBook = new Book("Mow the lawn");

            // Assert
            Assert.AreEqual(firstBook, secondBook);
        }

        [TestMethod]
        public void Save_SavesToDatabase_BookList()
        {
            //Arrange
            Book testBook = new Book("Mow the lawn");

            //Act
            testBook.Save();
            List<Book> result = Book.GetAll();
            List<Book> testList = new List<Book> { testBook };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            //Arrange
            Book testBook = new Book("Mow the lawn");

            //Act
            testBook.Save();
            Book savedBook = Book.GetAll()[0];

            int result = savedBook.GetId();
            int testId = testBook.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Edit_UpdatesBookInDatabase_String()
        {
            //Arrange
            Book testBook = new Book("Walk the Dog");
            testBook.Save();
            string secondDescription = "Mow the lawn";

            //Act
            testBook.Edit(secondDescription);
            string result = Book.Find(testBook.GetId()).GetBookTitle();

            //Assert
            Assert.AreEqual(secondDescription, result);
        }

        [TestMethod]
        public void Delete_DeletesBookAssociationFromDatabase_BookList()
        {
            //Arrange
            Author testAuthor = new Author("Home stuff");
            testAuthor.Save();
            string testDescription = "Mow the lawn";
            Book testBook = new Book(testDescription);
            testBook.Save();

            testBook.AddAuthor(testAuthor);
            testBook.Delete();
            List<Book> resultAuthorBooks = testAuthor.GetBooks();
            List<Book> testAuthorBooks = new List<Book> {};

            CollectionAssert.AreEqual(testAuthorBooks, resultAuthorBooks);
        }

        [TestMethod]
        public void GetCategories_ReturnsAllBookCategories_AuthorList()
        {
            Book testBook = new Book("Mow the lawn");
            testBook.Save();
            Author testAuthor1 = new Author("Home stuff");
            testAuthor1.Save();
            Author testAuthor2 = new Author("Work stuff");
            testAuthor2.Save();
            testBook.AddAuthor(testAuthor1);
            List<Author> result = testBook.GetAuthors();
            List<Author> testList = new List<Author>{testAuthor1};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void AddAuthor_AddsAuthorToBook_AuthorList()
        {
            Book newBook = new Book("Mow the lawn");
            newBook.Save();
            Author testAuthor = new Author("Home Stuff");
            testAuthor.Save();
            newBook.AddAuthor(testAuthor);
            List<Author> result = newBook.GetAuthors();
            List<Author> testList = new List<Author>{testAuthor};
            CollectionAssert.AreEqual(testList, result);
        }

        // [TestMethod]
        // public void GetAuthorId_ReturnsBooksParentAuthorId_Int()
        // {
        //   //Arrange
        //   Author newAuthor = new Author("Home Tasks");
        //   Book newBook = new Book("Walk the dog.", 1, newAuthor.GetId());
        //
        //   //Act
        //   int result = newBook.GetAuthorId();
        //
        //   //Assert
        //   Assert.AreEqual(newAuthor.GetId(), result);
        // }

    }
}
