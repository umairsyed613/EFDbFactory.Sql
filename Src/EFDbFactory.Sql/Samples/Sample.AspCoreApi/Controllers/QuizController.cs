using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspCoreApi.Controllers
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
        public async Task CreateQuiz(string name) => throw new NotImplementedException();

        [HttpPost]
        public async Task CreateQuestion(int quizId, string text) => throw new NotImplementedException();

        [HttpPost]
        public async Task CreateAnswer(int questionId, string text) => throw new NotImplementedException();

        [HttpPost]
        public async Task UpdateQuestionWithCorrectAnswerId(int questionId, int answerId) => throw new NotImplementedException();

        [HttpGet("[action]")]
        public async Task<IEnumerable<Quiz>> GetAllQuiz() => await _quizService.GetAllQuiz();
    }
}
