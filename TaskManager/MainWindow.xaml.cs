using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManagerRepo;
using TaskManagerRepo.Models;
using TaskManagerService;
using Windows.ApplicationModel.Appointments;
using Task = TaskManagerRepo.Models.Task;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User cur_session;
        private TaskRepo TaskRepo = new TaskRepo();
        public ObservableCollection<TaskManagerRepo.Models.Task> Tasks { get; set; }
        private ObservableCollection<String> TasksName { get; set; }
        private TaskService taskService;
        private DependencyService DependencyService;
        private ReminderService reminderService;
        private int isUpdate;

        public MainWindow(User logged_user)
        {
            InitializeComponent();
            cur_session = logged_user;
            Tasks = new ObservableCollection<TaskManagerRepo.Models.Task>(TaskRepo.GetTaskListFromUser(cur_session.UserId));
            taskService = new TaskService();
            DependencyService = new DependencyService();
            reminderService = new ReminderService();
            LoadData();
            
        }

        public void LoadData()
        {
            TasksDataGrid.ItemsSource = Tasks;
            AvailableTasksComboBox.ItemsSource = Tasks;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task task = TasksDataGrid.SelectedItem as Task;
                if (task != null)
                {
                    TabControlG.SelectedIndex = 1;

                    isUpdate = task.TaskId;

                    TitleTextBox.Text = task.Title;
                    DescriptionTextBox.Text = task.Description;
                    DueDatePicker.SelectedDate = task.DueDateTime;
                    PriorityComboBox.SelectedIndex = task.Priority == "Low" ? 0 : task.Priority == "Medium" ? 1 : task.Priority == "High" ? 2 : -1;
                    StatusComboBox.SelectedIndex = task.Status == "Pending" ? 0 : task.Status == "In Progress" ? 1 : task.Status == "Completed" ? 2 : -1;
                }
                else throw new Exception("Task is not selected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Add_Click(object sender, object e)
        {
            TabControlG.SelectedIndex = 1;

            isUpdate = 0;
        }

        private void DeleteTask_Click(object sender, object e)
        {
            try
            {
                Task task = TasksDataGrid.SelectedItem as Task;
                if (task != null)
                {
                    if (taskService.DeleteTask(task))
                    {
                        Tasks.Remove(task);
                        LoadData();
                    }
                    else throw new Exception("Task delete failed");
                } else throw new Exception("Task is not selected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Logout_Handler(object sender, object e)
        {
            cur_session = null;
            Login login = new Login();
            login.Show();
            this.Close();

        }

        private DateTime CalculateNextDueDate(DateTime lastDueDate, string recurrenceType)
        {
            switch (recurrenceType)
            {
                case "Daily":
                    return lastDueDate.AddDays(1);
                case "Weekly":
                    return lastDueDate.AddDays(7);
                case "Monthly":
                    return lastDueDate.AddMonths(1);
                case "Yearly":
                    return lastDueDate.AddYears(1);
                default:
                    return lastDueDate;
            }
        }

        private void ShowNotification(string taskTitle, DateTime reminderDateTime)
        {
            MessageBox.Show($"Reminder: Task '{taskTitle}' is due soon!", "Task Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void CheckReminders()
        {
            foreach (var reminder in reminderService.DueReminders())
            {
                ShowNotification(reminder.Task.Title, reminder.ReminderDateTime);
                reminderService.DeleteReminder(reminder);
            }
        }

        private void SaveTask_Click(object sender, object e)
        {
            try
            {
                string title = TitleTextBox.Text;
                string? description = DescriptionTextBox.Text;
                DateTime? dueDate = DueDatePicker.SelectedDate;
                string priority = ((ComboBoxItem)PriorityComboBox.SelectedItem)?.Content.ToString();
                string status = ((ComboBoxItem)StatusComboBox.SelectedItem)?.Content.ToString();
                string recurrence = ((ComboBoxItem)RecurrenceComboBox.SelectedItem)?.Content.ToString();
                var dependence_task = AvailableTasksComboBox.SelectedItem;
                string reminder_date = ((ComboBoxItem)AvailableTimer.SelectedItem)?.Content.ToString();
                bool timer = (bool)EnableTimer.IsChecked;

                Random rnd = new Random();


                Task new_task = new Task { 
                    Title = title,
                    UserId = this.cur_session.UserId,
                    Description = description, 
                    DueDateTime = dueDate, 
                    Priority = priority, 
                    Status = status, 
                    RecurrenceType = recurrence
                };

                if (timer && reminder_date.IsNullOrEmpty())
                {
                    throw new Exception("Reminder is not selected");
                }
                else if (timer && !reminder_date.IsNullOrEmpty())
                {
                    Reminder reminder = new Reminder { ReminderDateTime = DateTime.Now.AddMinutes(Double.Parse(reminder_date)) };
                    new_task.Reminders.Add(reminder);
                }

                if (isUpdate != 0)
                {
                    new_task.TaskId = isUpdate;
                    taskService.UpdateTask(new_task);
                } else
                {
                    taskService.SaveTask(new_task);
                    Tasks.Add(new_task);
                }

                TabControlG.SelectedIndex = 0;

                ClearInput();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearInput()
        {
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            DueDatePicker.SelectedDate = null;
            PriorityComboBox.SelectedIndex = -1;
            StatusComboBox.SelectedIndex = -1;
            RecurrenceComboBox.SelectedIndex = -1;
            AvailableTasksComboBox.SelectedIndex = -1;
            AvailableTimer.SelectedIndex = -1;
            EnableTimer.IsChecked = false;
        }

        private void CancelTask_Click(object sender, object e)
        {
            TabControlG.SelectedIndex = 0;

            ClearInput();
        }

        private void Remove_Dependency(object sender, object e)
        {
            if(AvailableTasksComboBox.SelectedItem != null)
            {
                AvailableTasksComboBox.SelectedItem = null;
                return;
            }
            MessageBox.Show("Dependency is empty");
        }


        private void Search_Handler(object sender, KeyEventArgs e)
        {

        }
    }
}