# Entity Framework Core Factory Pattern for Sql Server

[![NuGet version](https://badge.fury.io/nu/EFDbFactory.Sql.svg)](https://badge.fury.io/nu/EFDbFactory.Sql)

# Introduction 
Factory Pattern for Entity Framework Core. It helps for multiple EF DbContext with this pattern.
You can create readonly context and read-write with transaction.

# How to get it
You can get it from NuGet - just install the [EFDbFactory.Sql](https://www.nuget.org/packages/EFDbFactory.Sql/)

# How to use it

Inherit your dbcontext with commondbcontext 
```
public partial class YourDbContext : CommonDbContext
    {
        public YourDbContext(DbContextOptions<QuizDbContext> options)
            : base(options)
        {
        }
    }
```

Dependency Injection
```
services.AddSingleton<IFactoryCreator, FactoryCreator>(provider => new FactoryCreator(connectionString));
```

Injection in your controller
```
private readonly IFactoryCreator _factoryConn;

public WriteController(IFactoryCreator factoryConn)
{
  _factoryConn = factoryConn ?? throw new ArgumentNullException(nameof(factoryConn));
}
```
ReadWrite Factory
```
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
```
Readonly factory 
```
public async Task<IEnumerable<Book>> GetAllBooks()
        {
            using var factory = await _factoryCreator.Create();
            var context = factory.FactoryFor<BooksDbContext>().GetReadOnlyWithNoTracking();
            return context.Book.ToList();
        }
```

# Build and Test
Build Project and Run tests.

# Sample Projects
```
You can find sample projects under Src/Samples
```

# Feel Free to make it better
