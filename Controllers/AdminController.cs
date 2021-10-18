using iTransitionCourse.Data;
using iTransitionCourse.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTransitionCourse.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string a)
        {
            _db.TaskGroup.Add(new TaskGroup("Java"));
            _db.TaskGroup.Add(new TaskGroup("Math"));
            _db.TaskGroup.Add(new TaskGroup("History"));
            _db.TaskGroup.Add(new TaskGroup("Social"));
            _db.SaveChanges();
            return View();
        }
    }
}