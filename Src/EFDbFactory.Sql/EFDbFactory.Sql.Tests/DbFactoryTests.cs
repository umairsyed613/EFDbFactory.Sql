using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFDbFactory.Sql.Tests
{
    public class DbFactoryTests
    {
        private const string _connString = "TestDatabase";

        [Fact]
        public async Task TestMethod_UsingInMemoryProvider()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                         .UseInMemoryDatabase(_connString)
                         .Options;

            await using (var context = new TestDbContext(options))
            {
                var quiz = new Quiz() { Title = "Test 1" };
                context.Quiz.Add(quiz);
                await context.SaveChangesAsync();
            }

            await using (var context = new TestDbContext(options))
            {
                var q = Assert.Single(context.Quiz.ToList());
                Assert.NotNull(q);
                Assert.Equal("Test 1", q.Title);
            }
        }
    }
}
