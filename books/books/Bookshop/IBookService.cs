namespace books.BookShop;

public interface IBookService
{
    Task<Book> GetGenres(int id);
    Task<Book> AdBook(Book book);
}