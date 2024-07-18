using System;
using System.Collections.Generic;

namespace TaskManagerRepo.Models;

public partial class Task
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

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual ICollection<TaskDependency> TaskDependencyDependsOnTasks { get; set; } = new List<TaskDependency>();

    public virtual ICollection<TaskDependency> TaskDependencyTasks { get; set; } = new List<TaskDependency>();

    public virtual User User { get; set; } = null!;
}
