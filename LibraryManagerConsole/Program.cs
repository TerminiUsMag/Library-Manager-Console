using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.IO.Contracts;
using LibraryManagerConsole.Core.IO.Readers;
using LibraryManagerConsole.Core.IO.Writters;
using LibraryManagerConsole.Core.Services;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data;
using LibraryManagerConsole.Views;
using LibraryManagerConsole.Views.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<LibraryContext>();
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IWriter, ConsoleWriter>();
        services.AddScoped<IReader, ConsoleReader>();
        //services.AddScoped<IViews, Views>();

    })
    .Build();

var bookService = host.Services.GetService<IBookService>()!;
var writer = host.Services.GetService<IWriter>()!;
var reader = host.Services.GetService<IReader>()!;
//var views = host.Services.GetService<IViews>()!;

writer.WriteLine("Write a title for the book");
var bookTitle = reader.ReadLine().Trim();
writer.WriteLine("Write author's full name in the format : 'First name' 'Second name' 'Last name'");
var authorFullName = reader.ReadLine().Trim();
writer.WriteLine("Write book's release date in 'DD/MM/YYYY' format (default date is today)");
var bookReleaseDate = reader.ReadLine().Trim();
writer.WriteLine("Write the book's Genre/s (split them with semicolon ',') : ");
var bookGenres = reader.ReadLine().Trim().Split(',');

try
{
    var newBook = bookService.CreateFullBookModel(bookTitle, authorFullName, bookReleaseDate, bookGenres);
    writer.WriteLine(newBook);

    await bookService.AddBookAsync(newBook);
}
catch (ArgumentException aex)
{
    writer.WriteLine(aex.Message);
}

var books = await bookService.AllBooksReadOnlyAsync();
foreach (var book in books)
{
    writer.EmptyLine();
    writer.WriteLine(book);
}