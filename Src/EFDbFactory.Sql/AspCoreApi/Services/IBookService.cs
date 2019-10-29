using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCoreApi.Services
{
    public interface IBookService
    {
        Task CreateAuthor(string name, string email);
        Task CreateBook(int authorId, string title);
        Task<IEnumerable<Book>> GetAllBooks();
    }
}
