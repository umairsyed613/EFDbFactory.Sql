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

        private static IDbFactory GetNoCommitFactory() =>
            new DbFactory(_connString).CreateNoCommit().GetAwaiter().GetResult();

        [Fact]
        public async Task Test_NoCommitFactory_ThrowsExceptionWhenCommitingTransaction()
        {
            using var fac = GetNoCommitFactory();
            var context = fac.FactoryFor<TestDbContext>().GetReadWriteWithDbTransaction();

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
            using (var fac = GetNoCommitFactory())
            {
                var context = fac.FactoryFor<TestDbContext>().GetReadWriteWithDbTransaction();

                var quiz = new Quiz() {Title = "Test 1"};
                context.Quiz.Add(quiz);
                await context.SaveChangesAsync();

                var q = Assert.Single(context.Quiz.ToList());
                Assert.NotNull(q);
                Assert.Equal("Test 1", q.Title);
            }

            using (var fac2 = GetNoCommitFactory())
            {
                var context = fac2.FactoryFor<TestDbContext>().GetReadWriteWithDbTransaction();
                Assert.Empty(context.Quiz.ToList());
            }
        }
    }
}