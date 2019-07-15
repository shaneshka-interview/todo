using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Site.Models
{
    public class TodoItem
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime? Created { get; set; }

        public TimeSpan? Expire { get; set; }
    }
}