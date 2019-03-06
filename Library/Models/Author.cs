using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
    public class Author
    {
        private string _name;
        private int _id;

        public Author(string authorName, int id = 0)
        {
            _name = authorName;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM authors;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Author> GetAll()
        {
            List<Author> allAuthors = new List<Author> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM authors;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int AuthorId = rdr.GetInt32(1);
                string AuthorName = rdr.GetString(0);
                Author newAuthor = new Author(AuthorName, AuthorId);
                allAuthors.Add(newAuthor);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allAuthors;
        }

        public static Author Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM authors WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int AuthorId = 0;
            string AuthorName = "";
            while (rdr.Read())
            {
                AuthorId = rdr.GetInt32(1);
                AuthorName = rdr.GetString(0);
            }
            Author newAuthor = new Author(AuthorName, AuthorId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newAuthor;
        }

        public List<Book> GetBooks()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT books.* FROM authors
              JOIN authors_books ON (authors.id = authors_books.author_id)
              JOIN books ON (authors_books.book_id = books.id)
              WHERE authors.id = @AuthorId;";
          MySqlParameter authorIdParameter = new MySqlParameter();
          authorIdParameter.ParameterName = "@AuthorId";
          authorIdParameter.Value = _id;
          cmd.Parameters.Add(authorIdParameter);
          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          List<Book> books = new List<Book>{};
          while(rdr.Read())
          {
            int bookId = rdr.GetInt32(1);
            string bookName = rdr.GetString(0);
            Book newBook = new Book(bookName, bookId);
            books.Add(newBook);
          }
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return books;
        }

        public override bool Equals(System.Object otherAuthor)
        {
            if (!(otherAuthor is Author))
            {
                return false;
            }
            else
            {
                Author newAuthor = (Author)otherAuthor;
                bool idEquality = this.GetId().Equals(newAuthor.GetId());
                bool nameEquality = this.GetName().Equals(newAuthor.GetName());
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO authors (name) VALUES (@name);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
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
            MySqlCommand cmd = new MySqlCommand("DELETE FROM authors Where id = @AuthorId; DELETE FROM authors_books WHERE author_id = @AuthorId;", conn);
            MySqlParameter authorId = new MySqlParameter();
            authorId.ParameterName = "@AuthorId";
            authorId.Value = this.GetId();
            cmd.Parameters.Add(authorId);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddBook(Book newBook)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);";
            MySqlParameter author_id = new MySqlParameter();
            author_id.ParameterName = "@AuthorId";
            author_id.Value = _id;
            cmd.Parameters.Add(author_id);
            MySqlParameter book_id = new MySqlParameter();
            book_id.ParameterName = "@BookId";
            book_id.Value = newBook.GetId();
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
