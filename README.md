# Entity Framework Core Factory Pattern for Sql Server

[![NuGet version](https://badge.fury.io/nu/EFDbFactory.Sql.svg)](https://badge.fury.io/nu/EFDbFactory.Sql)
[![Build Status](https://travis-ci.org/umairsyed613/EFDbFactory.Sql.svg?branch=master)](https://travis-ci.org/umairsyed613/EFDbFactory.Sql)
[![Nuget downloads (EFDbFactory.Sql)](https://img.shields.io/nuget/dt/EFDbFactory.Sql)](https://nuget.org/packages/EFDbFactory.Sql)
![.NET](https://github.com/umairsyed613/EFDbFactory.Sql/workflows/.NET/badge.svg)

# Introduction 
Factory Pattern for Entity Framework Core. It helps for multiple EF DbContext with this pattern.
You can create readonly context and read-write with transaction.

# How to use it
### Important Inherit your dbcontext with EFDbContext 
```csharp
public partial class YourDbContext : EFDbContext
    {
        public YourDbContext(DbContextOptions<QuizDbContext> options)
            : base(options)
        {
        }
    }
```

Dependency Injection
```csharp
services.AddSingleton<IDbFactory, DbFactory>(provider => new DbFactory(connectionString));
```

ServiceCollection Extension
```csharp
Example 1 (No LoggerFactory)
	services.AddEfDbFactory(connectionString: Configuration.GetConnectionString("DbConnection"));

Example 2 (With LoggerFactory)
	services.AddEfDbFactory(connectionString: Configuration.GetConnectionString("DbConnection"), loggerFactory: MyLoggerFactory, enableSensitiveDataLogging: true);

Example 3 (With Options)
            services.AddEfDbFactory(new EfDbFactoryOptions
                                        {
                                            ConnectionString = Configuration.GetConnectionString("DbConnection"),
                                            LoggerFactory =  MyLoggerFactory,
                                            EnableSensitiveDataLogging = true
                                        });
```

Injection in your controller
```
private readonly IDbFactory _factoryConn;

public WriteController(IDbFactory factoryConn)
{
  _factoryConn = factoryConn ?? throw new ArgumentNullException(nameof(factoryConn));
}
```
ReadWrite Factory
```csharp
public async Task CreateBook(int authorId, string title)
        {
            using var factory = await factoryConn.Create(IsolationLevel.Snapshot);
            var context = factory.For<BooksDbContext>();

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
```csharp
public async Task<IEnumerable<Book>> GetAllBooks()
        {
            using var factory = await factoryConn.Create();
            var context = factory.For<BooksDbContext>();
            return context.Book.ToList();
        }
```

# Testing

```csharp
private const string _connString =
            "Server=localhost\\sqlexpress;Database=QuizDb;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=True;ConnectRetryCount=0";

private static async Task<IDbFactory> GetNoCommitFactoryAsync() => await new DbFactory(_connString).CreateNoCommit();

[Fact]
public async Task Test_NoCommitFactory_AutoRollBack()
{
    using (var fac = await GetNoCommitFactory())
    {
        var context = fac.For<TestDbContext>();

        var quiz = new Quiz() {Title = "Test 1"};
        context.Quiz.Add(quiz);
        await context.SaveChangesAsync();

        var q = Assert.Single(context.Quiz.ToList());
        Assert.NotNull(q);
        Assert.Equal("Test 1", q.Title);
    }

    using (var fac2 = await GetNoCommitFactory())
    {
        var context = fac2.For<TestDbContext>();
        Assert.Empty(context.Quiz.ToList());
    }
}

```

# Sample Projects
```
You can find sample projects under Src/Samples
```
