using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDbFactory.Sql;

namespace AspCoreApi.Services
{
    public class BookService : IBookService
    {
        private readonly IFactoryCreator _factoryCreator;

        public BookService(IFactoryCreator factoryCreator)
        {
            _factoryCreator = factoryCreator ?? throw new ArgumentNullException(nameof(factoryCreator));
        }

        public async Task CreateAuthor(string name, string email) => throw new NotImplementedException();

        public async Task CreateBook(int authorId, string title) => throw new NotImplementedException();

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            using var factory = await _factoryCreator.CreateFactoryWithNoTransaction();
            var context = factory.FactoryFor<BooksDbContext>().GetReadOnlyWithNoTracking();
            return context.Book.ToList();
        }
    }
}
