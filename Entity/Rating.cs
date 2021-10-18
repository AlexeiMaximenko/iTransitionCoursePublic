using System.ComponentModel.DataAnnotations;

namespace iTransitionCourse.Entity
{
    public class Rating
    {
        public int Id { get; set; }

        [Required]
        [Range(1,5)]
        public int Value { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public User User { get; set; }

    }
}