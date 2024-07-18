using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManagerRepo.Models;
using TaskManagerService;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;
                UserService userService = new UserService();
                User user = userService.Login(username, password);
                if (user != null)
                {
                    MainWindow main = new MainWindow(user);
                    main.Show();
                    this.Close();
                }
                else throw new Exception("Wrong Username or Password");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    string username = UsernameTextBox.Text;
                    string password = PasswordBox.Password;
                    UserService userService = new UserService();
                    User user = userService.Login(username, password);
                    if (user != null)
                    {
                        MainWindow main = new MainWindow(user);
                        main.Show();
                        this.Close();
                    }
                    else throw new Exception("Wrong Username or Password");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    }
}
