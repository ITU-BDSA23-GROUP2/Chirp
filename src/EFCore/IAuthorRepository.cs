namespace EFCore;

public interface IAuthorRepository
{
    public Task CreateNewAuthor(string name, string email);
    public Task<AuthorDto?> GetAuthorByID(int id);
    public Task<AuthorDto?> GetAuthorByName(string authorName);
    public Task<AuthorDto?> GetAuthorByEmail(string authorEmail);
    public Task UpdateAuthor(string oldName, string newName, string email);
    public Task DeleteAuthor(string name);
}