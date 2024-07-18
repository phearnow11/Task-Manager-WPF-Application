using System;
using System.Collections.Generic;

namespace TaskManagerRepo.Models;

public partial class Reminder
{
    public int ReminderId { get; set; }

    public int TaskId { get; set; }

    public DateTime ReminderDateTime { get; set; }

    public virtual Task Task { get; set; } = null!;
}
