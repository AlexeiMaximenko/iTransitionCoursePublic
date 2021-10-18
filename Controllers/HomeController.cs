using iTransitionCourse.Data;
using iTransitionCourse.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iTransitionCourse.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        
        public HomeController(AppDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            var tasks = _db.Tasks.Include(r => r.Rating).ToList();
            foreach (var item in tasks)
            {
                item.UpdateRating();
            }
            var users = _db.Users.Include(t => t.Tasks).Include(ct => ct.CompleteTasks).ToList();

            return View(users);
        }

        public async Task<IActionResult> CreateTestData()
        {
            var task1 = new Entity.Task
            {
                Title = "Первый таск",
                User = _db.Users.ToList().ElementAt(1),
                Description = "куку мир",
                RightAnswers = new List<string>() { "1" },
                CreateDate = DateTime.Now
            };
            var task2 = new Entity.Task
            {
                Title = "Таск 2",
                User = _db.Users.ToList().ElementAt(2),
                Description = "куку мир 2",
                RightAnswers = new List<string>() { "2" },
                CreateDate = DateTime.Now
            };
            var task3 = new Entity.Task
            {
                Title = "Третий таск",
                User = _db.Users.ToList().ElementAt(0),
                Description = "куку мир 3",
                RightAnswers = new List<string>() { "3" },
                CreateDate = DateTime.Now
            };
            _db.Tasks.Add(task1);
            _db.Tasks.Add(task2);
            _db.Tasks.Add(task3);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
