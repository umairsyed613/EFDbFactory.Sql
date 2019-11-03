using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task CreateBook(int authorId, string title)
        {
            using var factory = await _factoryCreator.Create(IsolationLevel.Snapshot);
            var context = factory.FactoryFor<BooksDbContext>().GetReadWriteWithDbTransaction();

            var book = new Book
            {
                Title = "New book",
                AuthorId = authorId
            };
            context.Book.Add(book);
            await context.SaveChangesAsync();
            factory.CommitTransaction();
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            using var factory = await _factoryCreator.Create();
            var context = factory.FactoryFor<BooksDbContext>().GetReadOnlyWithNoTracking();
            return context.Book.ToList();
        }
    }
}
