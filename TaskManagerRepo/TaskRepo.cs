using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo.Models;
using Task = TaskManagerRepo.Models.Task;

namespace TaskManagerRepo
{
    public class TaskRepo
    {

        private TaskManagerContext context = new TaskManagerContext();
        public TaskRepo() { }

        public List<Task> GetTaskListFromUser(int userid) { 
            return context.Tasks.Where(t => t.UserId == userid).ToList();
        }


        public void SaveTask(Task task)
        {
            context.Add(task);
            context.SaveChanges();
        }

        public void DeleteTask(Task task)
        {
            context.Tasks.Remove(task);
            context.SaveChanges(); 
        }

        public void UpdateTask(Task task)
        {
            context.Tasks.Update(task);
            context.SaveChanges();
        }

    }
}
