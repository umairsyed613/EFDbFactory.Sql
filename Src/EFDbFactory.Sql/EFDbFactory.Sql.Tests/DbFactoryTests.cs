using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EFDbFactory.Sql.Tests
{
    public class DbFactoryTests
    {
        private const string _connString =
            "Server=localhost\\sqlexpress;Database=QuizDb;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=True;ConnectRetryCount=0";

        private static async Task<IDbFactory> GetNoCommitFactoryAsync() => await new DbFactory(_connString).CreateNoCommit();

        [Fact]
        public async Task Test_NoCommitFactory_ThrowsExceptionWhenCommitingTransaction()
        {
            using var fac = await GetNoCommitFactoryAsync();
            var context = fac.For<TestDbContext>();

            var quiz = new Quiz() {Title = "Test 1"};
            context.Quiz.Add(quiz);
            await context.SaveChangesAsync();

            var q = Assert.Single(context.Quiz.ToList());
            Assert.NotNull(q);
            Assert.Equal("Test 1", q.Title);

            Assert.Throws<InvalidOperationException>(() => fac.CommitTransaction());
        }

        [Fact]
        public async Task Test_NoCommitFactory_AutoRollBack()
        {
            using (var fac = await GetNoCommitFactoryAsync())
            {
                var context = fac.For<TestDbContext>();

                var quiz = new Quiz() {Title = "Test 1"};
                context.Quiz.Add(quiz);
                await context.SaveChangesAsync();

                var q = Assert.Single(context.Quiz.ToList());
                Assert.NotNull(q);
                Assert.Equal("Test 1", q.Title);
            }

            using (var fac2 = await GetNoCommitFactoryAsync())
            {
                var context = fac2.For<TestDbContext>();
                Assert.Empty(context.Quiz.ToList());
            }
        }
    }
}