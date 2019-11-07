using System;
using System.Data;
using System.Linq;
using EFDbFactory.Sql;

namespace Sample.ConsoleAppCore
{
    class Program
    {
        private static readonly string _connectionString =
            "Server=localhost\\sqlexpress;Database=QuizDb;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=True;ConnectRetryCount=0";

        static void Main(string[] args)
        {
            using var factory = new DbFactory(_connectionString).Create().GetAwaiter().GetResult();
            var context = factory.FactoryFor<QuizDbContext>().GetReadOnlyWithNoTracking();
            ShowAllQuiz(context);

            Console.WriteLine("Provider Name of Quiz : ");
            var quizName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(quizName))
            {
                using var factory2 = new DbFactory(_connectionString).Create(IsolationLevel.ReadCommitted).GetAwaiter().GetResult();
                var writableContext = factory2.FactoryFor<QuizDbContext>().GetReadWriteWithDbTransaction();

                var quiz = new Quiz { Title = quizName };
                writableContext.Quiz.Add(quiz);
                writableContext.SaveChanges();
                factory2.CommitTransaction();
                ShowAllQuiz(writableContext);
            }


            Console.WriteLine("Press any key to exit..");
            Console.ReadKey();
        }

        private static void ShowAllQuiz(QuizDbContext context)
        {
            foreach (var quiz in context.Quiz.ToList()) { Console.WriteLine($"ID : {quiz.Id.ToString().PadRight(5)} Title: {quiz.Title}{Environment.NewLine}"); }
        }
    }
}
