using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Exceptions;
using TestingSystem.BLL.Interfaces;
using TestingSystem.BLL.Services;
using TestingSystem.Constants;
using TestingSystem.WEB.Models.Tests;

namespace TestingSystem.WEB.Controllers
{
    public class TestController : Controller
    {
        private ITestService TestService { get; }
        private IQuestionService QuestionService { get; }
        private IResultService ResultService { get; }

        public TestController(ITestService testService, IQuestionService questionService, IResultService resultService)
        {
            TestService = testService ?? throw new ArgumentNullException(nameof(testService));
            QuestionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            ResultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
        }

        // GET: Test
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await GetAllTests());
        }

        // GET: Test/IndexForAdmin
        [Authorize(Roles = RoleName.Admin)]
        public async Task<ActionResult> IndexForAdmin()
        {
            return View(await GetAllTests());
        }

        // GET: Test/CreateTest
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult CreateTest()
        {
            return View();
        }

        // POST: Test/CreateTest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTest([Bind(Include = "Id,Name,TestDescription")] TestViewModel testViewModel)
        {
            if (ModelState.IsValid)
            {
                var testDto = new TestDto
                {
                    Name = testViewModel.Name,
                    TestDescription = testViewModel.TestDescription,
                };

                try
                {
                    await TestService.CreateAsync(testDto);
                    return RedirectToAction("IndexForAdmin");
                }
                catch (TestException e)
                {
                    ViewBag.Error = e.Message;
                    return View("Error");
                }
            }

            return View(testViewModel);
        }


        // GET: Test/Edit/5
        [Authorize(Roles = RoleName.Admin)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var testDto = await TestService.GetByIdAsync(id);
            if (testDto == null)
                return HttpNotFound();

            TestViewModel testViewModel = new TestViewModel
            {
                Id = testDto.Id,
                Name = testDto.Name,
                TestDescription = testDto.TestDescription,
            };

            return View(testViewModel);
        }

        // POST: Test/Edit/5
        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,TestDescription")] TestViewModel testViewModel)
        {
            if (ModelState.IsValid)
            {
                var testDto = new TestDto
                {
                    Id = testViewModel.Id,
                    Name = testViewModel.Name,
                    TestDescription = testViewModel.TestDescription,
                };

                try
                {
                    await TestService.UpdateAsync(testDto);
                    return RedirectToAction("Details", "Test", new { id = testViewModel.Id });
                }
                catch (TestException e)
                {
                    ViewBag.Error = e.Message;
                    return View("Error");
                }
            }
            return View(testViewModel);
        }

        // GET: Test/Details/5
        [Authorize(Roles = RoleName.Admin)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var testDto = await TestService.GetByIdAsync(id.Value);
            if (testDto == null)
                return HttpNotFound();


            TestViewModel testViewModel = new TestViewModel
            {
                Id = testDto.Id,
                Name = testDto.Name,
                TestDescription = testDto.TestDescription,
                Questions = new List<QuestionViewModel>()
            };
            foreach (var question in testDto.Questions)
            {
                var newQuest = new QuestionViewModel
                {
                    Id = question.Id,
                    Point = question.Point,
                    TestId = question.TestId,
                    QuestionContent = question.QuestionContent,
                    Answers = new List<AnswerViewModel>()
                };
                foreach (var answer in question.Answers)
                {
                    var newAnsw = new AnswerViewModel
                    {
                        Id = answer.Id,
                        AnswerContent = answer.AnswerContent,
                        IsTrue = answer.IsTrue,
                        QuestionId = answer.QuestionId
                    };
                    newQuest.Answers.Add(newAnsw);
                }
                testViewModel.Questions.Add(newQuest);
            }

            return View(testViewModel);
        }

        [Authorize(Roles = RoleName.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            try
            {
                await TestService.DeleteAsync(id);
                return RedirectToAction("IndexForAdmin");
            }
            catch (TestException e)
            {
                ViewBag.Error = e.Message;
                return View("Error");
            }
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RunTest(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var testDto = await TestService.GetByIdAsync(id.Value);
            if (testDto == null)
                return HttpNotFound("Test not found");

            var questionDto = await QuestionService.GetFirstByTestIdAsync(id.Value);
            if (questionDto == null)
                return HttpNotFound("There are no questions in the test.");

            var questionView = new QuestionViewModel
            {
                Id = questionDto.Id,
                QuestionContent = questionDto.QuestionContent,
                Point = questionDto.Point,
                TestId = questionDto.TestId,
                Answers = new List<AnswerViewModel>()
            };
            foreach (var answerDto in questionDto.Answers)
            {
                var answerView = new AnswerViewModel
                {
                    Id = answerDto.Id,
                    QuestionId = answerDto.QuestionId,
                    AnswerContent = answerDto.AnswerContent,
                    IsTrue = answerDto.IsTrue
                };
                questionView.Answers.Add(answerView);
            }

            try
            {
                string userId = User.Identity.GetUserId();
                await ResultService.CreateTestResult(userId, testDto.Id);
            }
            catch (TestException e)
            {
                ViewBag.Error = e.Message;
                return View("Error");
            }

            ViewBag.TestName = testDto.Name;
            return View("ShowQuestion", questionView);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GoNextQuestion([Bind(Exclude = "Answers")] QuestionViewModel model, string testName, int[] answersId)
        {
            ViewBag.TestName = testName;
            if (ModelState.IsValid)
            {

                try
                {
                    string userId = User.Identity.GetUserId();
                    await ResultService.SaveQuestionResult(userId, model.TestId, model.Id, answersId);
                }
                catch (TestException e)
                {
                    ViewBag.Error = e.Message;
                    return View("Error");
                }

                var questionDto = await QuestionService.GetNextQuestion(model.TestId, model.Id);
                if (questionDto == null)
                    return RedirectToAction("GetResult", new { testId = model.TestId });

                var questionView = new QuestionViewModel
                {
                    Id = questionDto.Id,
                    QuestionContent = questionDto.QuestionContent,
                    Point = questionDto.Point,
                    TestId = questionDto.TestId,
                    Answers = new List<AnswerViewModel>()
                };
                foreach (var answerDto in questionDto.Answers)
                {
                    var answerView = new AnswerViewModel
                    {
                        Id = answerDto.Id,
                        QuestionId = answerDto.QuestionId,
                        AnswerContent = answerDto.AnswerContent,
                        IsTrue = answerDto.IsTrue
                    };
                    questionView.Answers.Add(answerView);
                }

                return View("ShowQuestion", questionView);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //return View("ShowQuestion", model);
        }

        [Authorize]
        public async Task<ActionResult> GetResult(int testId)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                var test = await TestService.GetByIdAsync(testId);


                string userId = User.Identity.GetUserId();
                var resultDto = await ResultService.GetTestResultAsync(userId, testId);
                var resultTest = new ResultViewModel
                {
                    TestName = test.Name,
                    Score = resultDto.SummaryResult,
                    UserName = userName,
                    IsPassed = resultDto.IsPassed
                };
                return View(resultTest);
            }
            catch (TestException e)
            {
                ViewBag.Error = e.Message;
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> TestSearch(string name)
        {
            var allTestDto = await TestService.GetAllAsync();

            var allTest = allTestDto.Where(t => t.Name.Contains(name)).ToList();
            var allTestViewModel = new List<TestViewModel>();
            foreach (var testDto in allTest)
            {
                allTestViewModel.Add(new TestViewModel
                {
                    Id = testDto.Id,
                    Name = testDto.Name,
                    TestDescription = testDto.TestDescription
                });
            }

            return PartialView("_TestSearch", allTestViewModel);
        }

        private async Task<List<TestViewModel>> GetAllTests()
        {
            var tests = await TestService.GetAllAsync();
            var testsViews = tests.Select(test => new TestViewModel
            {
                Id = test.Id,
                Name = test.Name,
                TestDescription = test.TestDescription,
            }).ToList();

            return testsViews;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TestService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}