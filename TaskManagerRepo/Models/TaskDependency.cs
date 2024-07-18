using System;
using System.Collections.Generic;

namespace TaskManagerRepo.Models;

public partial class TaskDependency
{
    public int DependencyId { get; set; }

    public int TaskId { get; set; }

    public int DependsOnTaskId { get; set; }

    public virtual Task DependsOnTask { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
