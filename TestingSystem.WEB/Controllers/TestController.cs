using System;
using System.Collections.Generic;
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
    public class TestController : Controller
    {
        private ITestService TestService { get; }

        public TestController(ITestService testService)
        {
            TestService = testService ?? throw new ArgumentNullException(nameof(testService));
        }

        // GET: Test
        public async Task<ActionResult> Index()
        {
            var tests = await TestService.GetAllAsync();
            var testsView = tests.Select(test => new TestViewModel
                {
                    Id = test.Id,
                    Name = test.Name,
                    TestDescription = test.TestDescription,
                }).ToList();

            return View(testsView);
        }

        // GET: Test/CreateTest
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
                await TestService.CreateAsync(testDto);
                return RedirectToAction("Index");
            }

            return View(testViewModel);
        }


        // GET: Test/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
                await TestService.UpdateAsync(testDto);
                return RedirectToAction("Index");
            }
            return View(testViewModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await TestService.DeleteAsync(id);
            return RedirectToAction("Index");
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