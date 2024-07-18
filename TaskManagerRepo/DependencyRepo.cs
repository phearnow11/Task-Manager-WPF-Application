using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo.Models;

namespace TaskManagerRepo
{
    public class DependencyRepo
    {
        private TaskManagerContext _context;
        public DependencyRepo() { _context = new TaskManagerContext(); }

        public void AddDependency(TaskDependency dependency)
        {
            _context.TaskDependencies.Add(dependency);
        }

        public void RemoveDependency(int dependency_id)
        {
            _context.TaskDependencies.Where(dependency => dependency.DependencyId == dependency_id).ExecuteDelete();
            _context.SaveChanges();
        }
    }
}
