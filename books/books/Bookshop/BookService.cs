using Microsoft.Data.SqlClient;

namespace books.BookShop;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly IConfiguration _configuration;

    public BookService(IBookRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }
    public async Task<Book> GetGenres(int id)
    {
        using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await con.OpenAsync();
        var prescription = await _repository.GetBookData(id, con);
        return prescription;
    }

    public async Task<Book> AdBook(Book book)
    {
        using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await con.OpenAsync();
        var prescription = await _repository.AdBookData(book, con);
        return prescription;
    }
}