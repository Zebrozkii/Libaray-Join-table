using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
    public class Book
    {
        private string _bookTitle;
        private int _id;

        public Book(string bookTitle, int id = 0)
        {
            _bookTitle = bookTitle;
            _id = id;
        }

        public string GetBookTitle()
        {
            return _bookTitle;
        }

        public void SetBookTitle(string newTitle)
        {
            _bookTitle = newTitle;
        }

        public int GetId()
        {
            return _id;
        }

        public static List<Book> GetAll()
        {
            List<Book> allBooks = new List<Book> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int bookId = rdr.GetInt32(1);
                string bookTitle = rdr.GetString(0);
                Book newBook = new Book(bookTitle, bookId);
                allBooks.Add(newBook);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBooks;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM books;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM books WHERE id = @bookId; DELETE FROM authors_books WHERE book_id = @bookId;";
            MySqlParameter bookIdParameter = new MySqlParameter();
            bookIdParameter.ParameterName = "@bookId";
            bookIdParameter.Value = this.GetId();
            cmd.Parameters.Add(bookIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static Book Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int bookId = 0;
            string bookTitle = "";
            while (rdr.Read())
            {
                bookId = rdr.GetInt32(1);
                bookTitle = rdr.GetString(0);
            }
            Book newBook = new Book(bookTitle, bookId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newBook;
        }

        public override bool Equals(System.Object otherBook)
        {
            if (!(otherBook is Book))
            {
                return false;
            }
            else
            {
                Book newBook = (Book)otherBook;
                bool idEquality = (this.GetId() == newBook.GetId());
                bool descriptionEquality = (this.GetBookTitle() == newBook.GetBookTitle());
                return (idEquality && descriptionEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO books (name) VALUES (@title);";
            MySqlParameter title = new MySqlParameter();
            title.ParameterName = "@title";
            title.Value = this._bookTitle;
            cmd.Parameters.Add(title);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit(string newTitle)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE books SET name = @newTitle WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter title = new MySqlParameter();
            title.ParameterName = "@newTitle";
            title.Value = newTitle;
            cmd.Parameters.Add(title);

            cmd.ExecuteNonQuery();
            _bookTitle = newTitle;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Author> GetAuthors()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT authors.* FROM books
                JOIN authors_books ON (books.id = authors_books.book_id)
                JOIN authors ON (authors_books.author_id = authors.id)
                WHERE books.id = @BookId;";
            MySqlParameter bookId = new MySqlParameter();
            bookId.ParameterName = "@BookId";
            bookId.Value = _id;
            cmd.Parameters.Add(bookId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Author> authors = new List<Author> {};
            while(rdr.Read())
            {
                int authorId = rdr.GetInt32(1);
                string authorName = rdr.GetString(0);
                Author foundAuthor = new Author(authorName, authorId);
                authors.Add(foundAuthor);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return authors;
        }

        public void AddAuthor(Author newAuthor)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);";
            MySqlParameter author_id = new MySqlParameter();
            author_id.ParameterName = "@AuthorId";
            author_id.Value = newAuthor.GetId();
            cmd.Parameters.Add(author_id);
            MySqlParameter book_id = new MySqlParameter();
            book_id.ParameterName = "@BookId";
            book_id.Value = _id;
            cmd.Parameters.Add(book_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
