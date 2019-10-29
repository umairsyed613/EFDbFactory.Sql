using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreApi.Services
{
    public interface IQuizService
    {
        Task CreateQuiz(string name);
        Task CreateQuestion(int quizId, string text);
        Task CreateAnswer(int questionId, string text);
        Task UpdateQuestionWithCorrectAnswerId(int questionId, int answerId);
        Task<IEnumerable<Quiz>> GetAllQuiz();
    }
}
