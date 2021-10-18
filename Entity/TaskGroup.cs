namespace iTransitionCourse.Entity
{
    public class TaskGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TaskGroup(string name)
        {
            Name = name;
        }
    }
}
