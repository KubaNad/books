using Microsoft.Data.SqlClient;

namespace books.BookShop;

public class BookRepository : IBookRepository
{
    public async Task<Book> GetBookData(int id, SqlConnection con)
    {
        List<string> genres = new List<string>();
        // List<Genre> genres = new List<Genre>();
        string sql1 =
            "Select name FROM genres inner join dbo.books_genres bg on genres.PK = bg.FK_genre where FK_book = @id";
        SqlCommand command1 = new SqlCommand(sql1, con);
        command1.Parameters.AddWithValue("@Id", id);
        using (SqlDataReader reader = await command1.ExecuteReaderAsync())
        {
            while (reader.Read())
            {
                genres.Add(reader.GetString(0));
            }
        }
        
        
        string sql2 =
            "Select PK, title FROM books WHERE PK = @id";
        SqlCommand command2 = new SqlCommand(sql2, con);
        command2.Parameters.AddWithValue("@Id", id);
        using (SqlDataReader reader = await command2.ExecuteReaderAsync())
        {
            if (reader.Read())
            {
                return new Book()
                {
                    PK = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Genres = genres
                };
            }
        }
        throw new ArgumentException();
    }

    public async Task<Book> AdBookData(Book book, SqlConnection con)
    {
        using var transaction = con.BeginTransaction();
        try
        {
            string sql2 = "INSERT INTO books (title) values (@title)";
            SqlCommand command2 = new SqlCommand(sql2, con, transaction);
            command2.Parameters.AddWithValue("@title", book.Title);
            command2.ExecuteNonQuery();

            foreach (var bookGenre in book.Genres)
            {
                int value = int.Parse(bookGenre);
                string instert = "INSERT INTO books_genres(FK_genre,FK_book) values (@bookGenre,(SELECT MAX(PK) FROM books))";
                SqlCommand command3 = new SqlCommand(instert, con, transaction);
                command3.Parameters.AddWithValue("@bookGenre", value);
                command3.ExecuteNonQuery();
            }
            
            
            
            
            List<string> genres = new List<string>();
            // List<Genre> genres = new List<Genre>();
            string sql1 =
                "Select name FROM genres inner join dbo.books_genres bg on genres.PK = bg.FK_genre where FK_book = (SELECT MAX(PK) FROM books)";
            SqlCommand command1 = new SqlCommand(sql1, con, transaction);
            using (SqlDataReader reader = await command1.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    genres.Add(reader.GetString(0));
                }
            }
        
        
            string sql5 =
                "Select PK, title FROM books WHERE PK = (SELECT MAX(PK) FROM books)";
            SqlCommand command5 = new SqlCommand(sql5, con, transaction);
            Book newBook = new Book();
            using (SqlDataReader reader = await command5.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    newBook = new Book()
                    {
                        PK = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Genres = genres
                    };
                }
            }
            
            transaction.Commit();
            return newBook;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}