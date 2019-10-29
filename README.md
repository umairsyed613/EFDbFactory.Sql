# Entity Framework Core Factory Pattern for Sql Server

# Introduction 
Factory Pattern for Entity Framework Core. It helps you use this for multiple EF DbContext with this pattern.
You can create readonly context and read-write with transaction

# How to get it
You can get it from NuGet - just install the [EFDbFactory.Sql](https://www.nuget.org/packages/EFDbFactory.Sql/)

# How to use it
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

public async Task AddBooks([FromBody] string name)
{
  using (var transaction = await _factoryConn.CreateFactoryWithTransaction(IsolationLevel.Snapshot))
  {
    var dbFactoryWithConnection = transaction.FactoryFor<MyDbContext>();
      using (var dbContext = dbFactoryWithConnection.GetReadWriteWithDbTransaction())
      {
          var book = new Book {
                          Name = name
                      };

          dbContext.Books.Add(book);
          await dbContext.SaveChanges();
      }
      transaction.commit();
  }
}
```

# Build and Test
Build Project and Run tests. 
