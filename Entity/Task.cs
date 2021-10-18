using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace iTransitionCourse.Entity
{
    public class Task
    {
        public string Id { get; set; }
        public User User { get; set; }
        public string TaskGroupId { get; set; }
        [Required]
        [MaxLength(80)]
        public string Title { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [Required]
        public List<string> RightAnswers { get; set; } = new List<string>();
        public List<string> PictureUri { get; set; } = new List<string>();
        public List<Rating> Rating { get; set; } = new List<Rating>();
        public DateTime CreateDate { get; set; }
        public double AvgRating { get; set; } = 0;
        public string AvatarUri { get; set; }
        public void UpdateRating()
        {
            if(Rating.Count != 0)
            {
            AvgRating = Rating.Average(r => r.Value);
            }
        }
    }
}
