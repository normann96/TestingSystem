using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Interfaces;
using TestingSystem.WEB.Models.Tests;

namespace TestingSystem.WEB.Controllers
{
    public class QuestionController : Controller
    {
        private IQuestionService QuestionService { get; }
        private ITestService TestService { get; }

        public QuestionController(IQuestionService questionService, ITestService testService)
        {
            QuestionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            TestService = testService ?? throw new ArgumentNullException(nameof(testService));
        }


        [HttpPost]
        public async Task<ActionResult> AddQuestion([Required] string name, int point, int id)
        {
            if (ModelState.IsValid)
            {
                var questionDto = new QuestionDto
                {
                    QuestionContent = name,
                    TestId = id,
                    Point = point,
                    Answers = new List<AnswerDto>()
                };
                await QuestionService.CreateAsync(questionDto);
                return RedirectToAction("Details", "Test", new { id = questionDto.TestId });
            }
            return PartialView("_AddQuestion", new QuestionViewModel{QuestionContent = name, TestId = id, Point = point});
        }


        [HttpPost]
        public async Task<ActionResult> DeleteQuestion([Required] int? questionId, [Required] int? testId)
        {
            if(testId == null)
                return RedirectToAction("Index", "Test");

            if (questionId != null)
                await QuestionService.DeleteAsync(questionId.Value);
            
            return RedirectToAction("Details", "Test", new { id = testId.Value });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                QuestionService?.Dispose();
                TestService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}