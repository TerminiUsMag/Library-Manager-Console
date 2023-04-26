using LibraryManagerConsole.Core.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IAuthorService
    {
        Task AddAuthorAsync();
        Task DeleteAuthorAsync();
        Task FindAuthorAsync(int id);
        Task FindAuthorAsync(string name);
        Task UpdateAuthorAsync(AuthorViewModel author);

    }
}
