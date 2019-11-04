using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.AspCoreApiWithLogger.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sample.AspCoreApiWithLogger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController : ControllerBase, IQuizService
    {
        private readonly ILogger<QuizController> _logger;
        private readonly IQuizService _quizService;

        public QuizController(ILogger<QuizController> logger, IQuizService quizService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
        }

        [HttpPost]
        public async Task CreateQuiz(string name) => await _quizService.CreateQuiz(name);

        [HttpPost]
        public async Task CreateQuestion(int quizId, string text) => await _quizService.CreateQuestion(quizId, text);

        [HttpPost]
        public async Task CreateAnswer(int questionId, string text) => await _quizService.CreateAnswer(questionId, text);

        [HttpPost]
        public async Task UpdateQuestionWithCorrectAnswerId(int questionId, int answerId) => throw new NotImplementedException();

        [HttpGet("[action]")]
        public async Task<IEnumerable<Quiz>> GetAllQuiz() => await _quizService.GetAllQuiz();
    }
}
