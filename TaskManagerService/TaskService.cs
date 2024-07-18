using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo;
using TaskManagerRepo.Models;
using Task = TaskManagerRepo.Models.Task;

namespace TaskManagerService
{
    public class TaskService
    {

        private TaskRepo taskRepo = new TaskRepo();

        public TaskService() { }

        public List<Task> GetTaskFromUser(int userid)
        {
            return taskRepo.GetTaskListFromUser(userid);
        }

        public void SaveTask(Task task)
        {
            taskRepo.SaveTask(task);
        }

        public bool DeleteTask(Task task)
        {
            try
            {
                taskRepo.DeleteTask(task);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateTask(Task task)
        {
            taskRepo.UpdateTask(task);
        }
    }
}
