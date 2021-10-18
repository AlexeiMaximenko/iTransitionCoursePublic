using iTransitionCourse.Data;
using iTransitionCourse.Model;
using iTransitionCourse.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iTransitionCourse.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        public TaskController(AppDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _db = context;
        }

        public async Task<ActionResult> Details(string id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
            var taskModel = new TaskResolveViewModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description
            };
            return View(taskModel);
        }

        public async Task<ActionResult> Resolve(string id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
            var taskModel = new TaskResolveViewModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AvgRating = task.AvgRating.ToString()
            };

            return View(taskModel);
        }

        [HttpPost]
        public async Task<ActionResult> Resolve(TaskResolveViewModel model)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == model.Id);
            var userIncludeCompleteTask = _db.Users.Include(ct => ct.CompleteTasks).ToList().First(u => u.UserName == User.Identity.Name);
            var msg = "The answer is wrong!";
            foreach (var rightAnswer in task.RightAnswers)
            {
                if (rightAnswer == model.Answer)
                {
                    if (userIncludeCompleteTask.CompleteTasks.Tasks.Contains(task))
                    {
                        msg = "You have already done this task!";
                        break;
                    }
                    msg = "The answer is correct!";
                    userIncludeCompleteTask.CompleteTasks.Tasks.Add(task);
                    _db.SaveChanges();
                }
            }
            model.Msg = msg;
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return base.View(new Entity.Task());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Entity.Task model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.User = _db.Users.Include(t => t.CompleteTasks).ToList().Find(u => u.UserName == User.Identity.Name);
            _db.Tasks.Add(model);
            _db.SaveChanges();

            return Redirect("/Login/Profile");
        }

        [HttpPost]
        [Authorize]
        public IActionResult SetRating(string id, int ratingValue)
        {
            var task = _db.Tasks.Include(r => r.Rating).ToList().Find(t => t.Id == id);
            var user = _db.Users.ToList().Find(u => u.UserName == User.Identity.Name);
            foreach (var rate in task.Rating)
            {
                if(rate.User == user)
                {
                    rate.Value = ratingValue;
                    _db.SaveChanges();
                    task.UpdateRating();
                    return Redirect($"/Task/Details/{id}");
                }
            }
            task.Rating.Add(new Rating() { Value = ratingValue, User = user});
            _db.SaveChanges();
            task.UpdateRating();
            return Redirect($"/Task/Details/{id}");
        }

        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == id);

            return View(task);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Entity.Task changeTask)
        {
            var editTask = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == changeTask.Id);
            editTask.Title = changeTask.Title;
            editTask.Description = changeTask.Description;
            editTask.CreateDate = DateTime.Now;
            editTask.RightAnswers = changeTask.RightAnswers;
            await _db.SaveChangesAsync();
            return Redirect("/Login/Profile/");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return Redirect("/Login/Profile/");
        }
    }
}
