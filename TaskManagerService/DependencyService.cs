using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo;
using TaskManagerRepo.Models;

namespace TaskManagerService
{
    public class DependencyService
    {
        DependencyRepo repo;
        public DependencyService() { repo = new DependencyRepo(); }

        public void AddDependency(TaskDependency dependency) {  repo.AddDependency(dependency); }

        public void RemoveDependency(TaskDependency dependency) { repo.RemoveDependency(dependency.DependencyId); }
    }
}
