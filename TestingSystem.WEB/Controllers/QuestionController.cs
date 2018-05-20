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
using TestingSystem.Constants;
using TestingSystem.WEB.Models.Tests;

namespace TestingSystem.WEB.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
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
        public async Task<ActionResult> AddQuestion([Bind(Exclude = "Answers")] QuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var questionDto = new QuestionDto
                {
                    QuestionContent = viewModel.QuestionContent,
                    TestId = viewModel.TestId,
                    Point = viewModel.Point,
                    Answers = new List<AnswerDto>()
                };
                await QuestionService.CreateAsync(questionDto);
                return RedirectToAction("Details", "Test", new { id = questionDto.TestId });
            }
            return PartialView("_AddQuestion", viewModel);
        }


        // GET: Question/Edit/5
        public async Task<ActionResult> Edit(int? questionId)
        {
            if (questionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var questionDto = await QuestionService.GetByIdAsync(questionId.Value);
            if (questionDto == null)
            {
                return HttpNotFound();
            }
            QuestionViewModel questionViewModel = new QuestionViewModel
            {
                Id = questionDto.Id,
                QuestionContent = questionDto.QuestionContent,
                TestId = questionDto.TestId,
                Point = questionDto.Point,
                Answers = new List<AnswerViewModel>()
            };
            foreach (var answer in questionDto.Answers)
            {
                var newAnswer = new AnswerViewModel
                {
                    Id = answer.Id,
                    AnswerContent = answer.AnswerContent,
                    IsTrue = answer.IsTrue,
                    QuestionId = answer.QuestionId
                };
                (questionViewModel.Answers as List<AnswerViewModel>)?.Add(newAnswer);
            }

            return View(questionViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(QuestionViewModel questionViewModel)
        {
            if (ModelState.IsValid)
            {
                var questDto = new QuestionDto
                {
                    Id = questionViewModel.Id,
                    Point = questionViewModel.Point,
                    TestId = questionViewModel.TestId,
                    QuestionContent = questionViewModel.QuestionContent,
                    Answers = new List<AnswerDto>()
                };

                if (questionViewModel.Answers != null)
                {
                    foreach (var answer in questionViewModel.Answers)
                    {
                        questDto.Answers.Add(new AnswerDto
                        {
                            Id = answer.Id,
                            AnswerContent = answer.AnswerContent,
                            IsTrue = answer.IsTrue,
                            QuestionId = answer.QuestionId
                        });
                    }
                }
                questionViewModel.Answers = new List<AnswerViewModel>();
                await QuestionService.UpdateAsync(questDto);
                return RedirectToAction("Edit", "Question", new { questionId = questDto.Id });
            }
            return View(questionViewModel);
        }


        [HttpPost]
        public async Task<ActionResult> AddAnswer(int? testId, int? id, int? count)
        {
            if (testId == null || id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var question = await QuestionService.GetByIdAsync(id.Value);
            if (question == null)
                return HttpNotFound("Question not found");

            AnswerViewModel answerView = new AnswerViewModel { QuestionId = id.Value};
            ViewBag.QuestionName = question.QuestionContent;
            
            ViewBag.CountAnswer = count;
            return PartialView("_AddAnswer", answerView);
        }


        [HttpPost]
        public async Task<ActionResult> DeleteQuestion([Required] int? questionId, [Required] int? testId)
        {
            if (testId == null || questionId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            await QuestionService.DeleteAsync(questionId.Value);
            return RedirectToAction("Details", "Test", new { id = testId.Value });
        }


        [HttpGet]
        public async Task<ActionResult> DeleteAnswer([Required] int? answerId)
        {
            if (answerId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var answerDto = await QuestionService.GetAnswerByIdAsync(answerId.Value);
            if (answerDto == null)
                return HttpNotFound();
            
            AnswerViewModel answerViewModel = new AnswerViewModel
            {
                Id = answerDto.Id,
                AnswerContent = answerDto.AnswerContent,
                IsTrue = answerDto.IsTrue,
                QuestionId = answerDto.QuestionId
            };
            return View(answerViewModel);
        }


        // POST: Question/Delete/5
        [HttpPost, ActionName("DeleteAnswer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int answerId, int questionId)
        {
            await QuestionService.DeleteAnswerAsync(answerId);
            return RedirectToAction("Edit", "Question", new { questionId });
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