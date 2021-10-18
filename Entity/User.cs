using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace iTransitionCourse.Entity
{
    public class User : IdentityUser<string>
    {
        public string ThemeId { get; set; }
        public string AvatarUri { get; set; }
        public CompleteTask CompleteTasks { get; set; } = new CompleteTask();
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
