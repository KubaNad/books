using Microsoft.Data.SqlClient;

namespace books.BookShop;

public interface IBookRepository
{
    Task<Book> GetBookData(int id, SqlConnection con);
    Task<Book> AdBookData(Book book, SqlConnection con);
}