using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EFDbFactory.Sql;

namespace Sample.AspCoreApiWithLogger.Services
{
    public class QuizService : IQuizService
    {
        private readonly IDbFactory _dbFactory;

        public QuizService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task CreateQuiz(string name)
        {
            using var factory = await _dbFactory.Create(IsolationLevel.ReadCommitted);
            var context = factory.For<QuizDbContext>();
            var q = new Quiz { Title = name };
            context.Quiz.Add(q);
            await context.SaveChangesAsync();
            factory.CommitTransaction();
        }

        public async Task CreateQuestion(int quizId, string text) => throw new NotImplementedException();

        public async Task CreateAnswer(int questionId, string text) => throw new NotImplementedException();

        public async Task UpdateQuestionWithCorrectAnswerId(int questionId, int answerId) => throw new NotImplementedException();

        public async Task<IEnumerable<Quiz>> GetAllQuiz()
        {
            using var factory = await _dbFactory.Create();
            var context = factory.For<QuizDbContext>();
            return context.Quiz.ToList();
        }
    }
}
