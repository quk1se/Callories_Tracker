using Callories_Tracker.Data;
using Callories_Tracker.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        private string username;
        private DataContext dataContext;
        public static string my_id = null!;
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
                Dispatcher.Invoke(() =>
                {
                    my_id = dataContext
                    .Accounts
                    .Where(acc => acc.Mail == accountMailTextBox.Text)
                    .Select(acc => acc.Id)
                    .FirstOrDefault().ToString();
                });
                this.Hide();
                new MainWindow().ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect mail or pass!", "Sign in information", MessageBoxButton.OK, MessageBoxImage.Error);
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
            string emailPattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,7}\b";
            string passwordPattern = @".{6,}";
            string namePattern = @"^([^\@]+)";
            string statsPattern = @"^\d+$";
            Match match = Regex.Match(accountNewMailTextBox.Text, namePattern);
            username = match.Groups[1].Value;
            if (Regex.IsMatch(accountNewMailTextBox.Text, emailPattern) &&
                Regex.IsMatch(accountNewPasswordTextBox.Text, passwordPattern) &&
                Regex.IsMatch(accountNewWeightTextBox.Text, statsPattern) &&
                Regex.IsMatch(accountNewHeightTextBox.Text, statsPattern) &&
                Regex.IsMatch(accountNewAgeTextBox.Text, statsPattern))
            {
                Task task = new Task(AddNewAccount);
                task.Start();
                MessageBox.Show("Account successfully created!", "Registration information", MessageBoxButton.OK, MessageBoxImage.Information);
                signUpGrid.Visibility = Visibility.Hidden;
                signInGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Enter valid data!", "Registration information", MessageBoxButton.OK, MessageBoxImage.Error);
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
                accountNewMailTextBox.Foreground = Brushes.DarkRed;
                signUpRegistrationBtn.Foreground = Brushes.Gray;
            }
            else
            {
                mailLabel.Foreground = Brushes.DarkOliveGreen;
                signUpRegistrationBtn.IsEnabled = true;
                mailRect.Stroke = Brushes.DarkOliveGreen;
                accountNewMailTextBox.Foreground = Brushes.DarkOliveGreen;
                signUpRegistrationBtn.Foreground = Brushes.DarkOliveGreen;
            }
        }

        public void AddNewAccount()
        {
            Dispatcher.Invoke(() =>
            {
                Data.Entity.Account account = new Account
                {
                    Id = Guid.NewGuid(),
                    Mail = accountNewMailTextBox.Text,
                    Password = accountNewPasswordTextBox.Text,
                    Name = username
                };
                Data.Entity.Stat stat = new Stat
                {
                    Id = Guid.NewGuid(),
                    Account_Id = account.Id.ToString(),
                    Weight = accountNewWeightTextBox.Text,
                    Height = accountNewHeightTextBox.Text,
                    Age = accountNewAgeTextBox.Text,
                    Max_Target = "3000",
                    Picture = null
                };
                Data.Entity.Achievements achievements = new Achievements
                {
                    AccountId = account.Id.ToString(),
                    StartOfALongJourney = "true",
                    FirstTarget = "false",
                    StreakOfThree = "false",
                    StreakOfFive = "false",
                    StreakOfTen = "false",
                    CompleteYourProfile = "false",
                    LoverOfMotivation = "false",
                    DoublePortion = "false",
                    TriplePortion = "false",
                    AccuracyToTheMillimeter = "false",
                    LeaveMeAlone = "false",
                    Epilepsy = "false",
                };
                dataContext.Accounts.Add(account);
                dataContext.Achievements.Add(achievements);
                dataContext.Stats.Add(stat);
                dataContext.SaveChanges();
            });
        }
    }
}
