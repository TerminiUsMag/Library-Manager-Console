using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Core.Services;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data;
using LibraryManagerConsole.IO.Contracts;
using LibraryManagerConsole.IO.Readers;
using LibraryManagerConsole.IO.Writters;
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
    })
    .Build();

var bookService = host.Services.GetService<IBookService>()!;
var writer = host.Services.GetService<IWriter>()!;
var reader = host.Services.GetService<IReader>()!;
writer.WriteLine("Write a title for the book");
var bookTitle = reader.ReadLine().Trim();
writer.WriteLine("Write author's first name");
var authorFirstName = reader.ReadLine().Trim();
writer.WriteLine("Write author's middle name");
var authorMiddleName = reader.ReadLine().Trim();
writer.WriteLine("Write author's last name");
var authorLastName = reader.ReadLine().Trim();
writer.WriteLine("Write the book's Genre/s (split them with semicolon ',') : ");
var bookGenres = reader.ReadLine().Trim().Split(',');



try
{
    var newBook = bookService.CreateFullBookModel(bookTitle, authorFirstName, authorMiddleName, authorLastName, bookGenres);
    writer.WriteLine(newBook);

    await bookService.AddBookAsync(newBook);
}
catch (ArgumentException aex)
{
    writer.WriteLine(aex.Message);
}
writer.WriteLine("Completed second part\n");
var books = await bookService.AllBooksAsync();
foreach (var book in books)
{
    writer.EmptyLine();
    writer.WriteLine(book);
}

