using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Core.Services;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data;
using LibraryManagerConsole.IO.Contracts;
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
        services.AddScoped<IWritter, ConsoleWriter>();
    })
    .Build();

var bookService = host.Services.GetService<IBookService>()!;
var writter = host.Services.GetService<IWritter>()!;
writter.WriteLine("Write a title for the book");
var bookTitle = Console.ReadLine()!.Trim();
writter.WriteLine("Write author's first name");
var authorFirstName = Console.ReadLine()!.Trim();
writter.WriteLine("Write author's middle name");
var authorMiddleName = Console.ReadLine()!.Trim();
writter.WriteLine("Write author's last name");
var authorLastName = Console.ReadLine()!.Trim();
writter.WriteLine("Write the book's Genre/s (split them with semicolon ',') : ");
var bookGenres = Console.ReadLine()!.Trim().Split(',');



try
{
    var newBook = bookService.CreateFullBookModel(bookTitle, authorFirstName, authorMiddleName, authorLastName, bookGenres);
    writter.WriteLine(newBook);

    await bookService.AddBookAsync(newBook);
}
catch (ArgumentException aex)
{
    Console.WriteLine(aex.Message);
}
Console.WriteLine("Completed second part\n");
var books = await bookService.AllBooksAsync();
foreach (var book in books)
{
    Console.WriteLine(book.Title);
    Console.WriteLine($"Author -> {book.Author.ToString()}");
    Console.WriteLine(book.DateOfRelease);
    if (book.Genres.Count > 0)
        Console.WriteLine($"Genres -> {book.Genres.Select(g => g.ToString())}");
}
Console.WriteLine("Completed first part\n");

