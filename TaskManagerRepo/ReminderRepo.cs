using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo.Models;

namespace TaskManagerRepo
{
    public class ReminderRepo
    {
        TaskManagerContext context;
        public ReminderRepo() { context = new TaskManagerContext(); }

        public dynamic DueReminder()
        {
            var dueReminders = context.Reminders.Include(r => r.Task).Where(r => r.ReminderDateTime <= DateTime.Now).ToList();
            return dueReminders;
        }

        public void DeleteReminder(Reminder reminder)
        {
            context.Reminders.Remove(reminder);
            context.SaveChanges();
        }
    }
}
