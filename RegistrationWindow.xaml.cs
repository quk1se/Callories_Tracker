using Callories_Tracker.Data;
using Callories_Tracker.Data.Entity;
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

namespace Callories_Tracker
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private DataContext dataContext;
        Brain brain = new Brain();
        public RegistrationWindow()
        {
            InitializeComponent();
            dataContext = new();
        }

        private void signInBtn_Click(object sender, RoutedEventArgs e)
        {
            var account = dataContext.Accounts
                .FirstOrDefault(acc => acc.Mail == accountMailTextBox.Text && acc.Password == accountPasswordTextBox.Text);
            if (account != null)
            {
                this.Hide();
                new MainWindow().ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect mail or pass!", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void signUpBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            signUpBtn.Foreground = Brushes.White;
        }

        private void signUpBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            signUpBtn.Foreground = Brushes.DarkOliveGreen;
        }

        private void signUpRegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (accountNewMailTextBox.Text != "" || accountNewPasswordTextBox.Text != "" || accountNewNameTextBox.Text != "" )
            {
                Data.Entity.Account account = new Account
                {
                    Id = Guid.NewGuid(),
                    Mail = accountNewMailTextBox.Text,
                    Password = accountNewPasswordTextBox.Text,
                    Name = accountNewNameTextBox.Text
                };
                dataContext.Accounts.Add(account);
                dataContext.SaveChanges();
            }
        }

        private void signInRegistrationBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            signInRegistrationBtn.Foreground = Brushes.White;
        }

        private void signInRegistrationBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            signInRegistrationBtn.Foreground = Brushes.DarkOliveGreen;
        }

        private void signInRegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            signUpGrid.Visibility = Visibility.Hidden;
            signInGrid.Visibility = Visibility.Visible;
        }

        private void signUpBtn_Click(object sender, RoutedEventArgs e)
        {
            signUpGrid.Visibility = Visibility.Visible;
            signInGrid.Visibility = Visibility.Hidden;
        }

        private void accountNewMailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var account = dataContext.Accounts
                .FirstOrDefault(acc => acc.Mail == accountNewMailTextBox.Text);
            if (account != null)
            {
                mailLabel.Foreground = Brushes.DarkRed;
                signUpRegistrationBtn.IsEnabled = false;
                mailRect.Stroke = Brushes.DarkRed;
                signUpRegistrationBtn.Foreground = Brushes.Gray;
            }
            else
            {
                mailLabel.Foreground = Brushes.DarkOliveGreen;
                signUpRegistrationBtn.IsEnabled = true;
                mailRect.Stroke = Brushes.DarkOliveGreen;
                signUpRegistrationBtn.Foreground = Brushes.DarkOliveGreen;
            }
        }
    }
}
