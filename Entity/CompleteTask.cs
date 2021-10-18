using System.Collections.Generic;

namespace iTransitionCourse.Entity
{
    public class CompleteTask
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
