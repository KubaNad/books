namespace books.BookShop;

public static class ConfigurationForBookshop
{
    public static void RegisterEndpointsForBookshop(this IEndpointRouteBuilder app)
    {


        app.MapGet("/api/books/{id}/genres", async (int id, IBookService service) =>
            // await service.GetPrescription(id) is Prescription prescription ? Results.Ok(prescription) : Results.NotFound());
            Results.Ok(await service.GetGenres(id)));

        app.MapPost("/api/books", async (Book book, IBookService service) =>
        {
            try
            {
                var newPrescription = await service.AdBook(book);
                return Results.Ok(newPrescription);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}