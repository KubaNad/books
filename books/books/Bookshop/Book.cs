namespace books.BookShop;

public class Book
{
    public int PK { get; set; }
    public string Title { get; set; }
    public List<string> Genres { get; set; }
}