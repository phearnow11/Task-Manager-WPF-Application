using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManagerRepo.Models
{
    internal class TaskViewModel
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDateTime { get; set; }

        public string? Priority { get; set; }

        public string? Status { get; set; }

        public string? RecurrenceType { get; set; }

        public int? RecurrenceInterval { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}
