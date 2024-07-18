using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo;
using TaskManagerRepo.Models;

namespace TaskManagerService
{
    public class ReminderService
    {
        ReminderRepo repo;
        public ReminderService() { repo = new ReminderRepo(); }

        public dynamic DueReminders()
        {
            return repo.DueReminder();
        }

        public void DeleteReminder(Reminder reminder)
        {
            repo.DeleteReminder(reminder);
        }
    }
}
