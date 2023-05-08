using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.IO.Contracts;
using LibraryManagerConsole.Core.IO.Readers;
using LibraryManagerConsole.Core.IO.Writters;
using LibraryManagerConsole.Core.Services;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data;
using LibraryManagerConsole.Views;
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

Views.CreateBook(writer, reader, bookService);

var books = await bookService.AllBooksReadOnlyAsync();
foreach (var book in books)
{
    writer.EmptyLine();
    writer.WriteLine(book);
}

