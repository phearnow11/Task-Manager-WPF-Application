using System;
using System.Linq;
using System.Timers;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using TaskManagerRepo.Models;
using Task = TaskManagerRepo.Models.Task;

namespace TaskManager
{
    public class ReminderManager
    {
        private System.Timers.Timer _timer;
        private MainWindow _mainWindow;

        public ReminderManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _timer = new System.Timers.Timer(60000); // Check every minute
            _timer.Elapsed += CheckReminders;
            _timer.Start();
        }

        private void CheckReminders(object sender, ElapsedEventArgs e)
        {
            var tasksToRemind = _mainWindow.Tasks.Where(t =>
                t.DueDateTime.HasValue &&
                t.DueDateTime.Value.Date == DateTime.Today &&
                t.DueDateTime.Value.Hour == DateTime.Now.Hour &&
                t.DueDateTime.Value.Minute == DateTime.Now.Minute &&
                t.Status != "Completed"
            ).ToList();

            foreach (var task in tasksToRemind)
            {
                ShowNotification(task);
            }
        }

        private void ShowNotification(Task task)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var textNodes = toastXml.GetElementsByTagName("text");
            textNodes[0].AppendChild(toastXml.CreateTextNode("Task Reminder"));
            textNodes[1].AppendChild(toastXml.CreateTextNode($"'{task.Title}' is due now!"));

            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier("TaskManager").Show(toast);
        }
    }
}