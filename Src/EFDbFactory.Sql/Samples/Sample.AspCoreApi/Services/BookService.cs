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
        private readonly IDbFactory _dbFactory;

        public BookService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task CreateAuthor(string name, string email) => throw new NotImplementedException();

        public async Task CreateBook(int authorId, string title)
        {
            using var factory = await _dbFactory.Create(IsolationLevel.Snapshot);
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
            using var factory = await _dbFactory.Create();
            var context = factory.FactoryFor<BooksDbContext>().GetReadOnlyWithNoTracking();
            return context.Book.ToList();
        }
    }
}
