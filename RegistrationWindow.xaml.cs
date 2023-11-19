using Callories_Tracker.Data;
using Callories_Tracker.Data.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private bool hiddenPassword = false;
        public RegistrationWindow()
        {
            InitializeComponent();
            dataContext = new();
        }

        private void signInBtn_Click(object sender, RoutedEventArgs e)
        {
            var confCode = dataContext.Accounts.FirstOrDefault(acc => acc.Mail == accountMailTextBox.Text);
            var account = dataContext.Accounts
                .FirstOrDefault(acc => acc.Mail == accountMailTextBox.Text && (acc.Password == accountHidePasswordTextBox.Password || acc.Password == 
                accountPasswordTextBox.Text));
            if (account != null)
            {
                if (confCode.ConfirmCode == null)
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
                    using SmtpClient? smtpClient = GetSmtpClient();
                    smtpClient?.Send(
                        App.GetConfiguration("smtp:email")!,
                        accountMailTextBox.Text,
                        "Signup successfull",
                        $"Congratulations! To confirm Email use code: {confCode.ConfirmCode}"
                    );
                    MessageBox.Show("Confirm your code from email!", "Email confiramtion", MessageBoxButton.OK, MessageBoxImage.Warning);
                    confirmCodeBtn.Visibility = Visibility.Visible;
                    confirmCodeRect.Visibility = Visibility.Visible;
                    confirmCodeTextBox.Visibility = Visibility.Visible;
                    signInBtn.IsEnabled = false;
                }
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
                String code = Guid.NewGuid().ToString()[..6].ToUpper();
                Data.Entity.Account account = new Account
                {
                    Id = Guid.NewGuid(),
                    Mail = accountNewMailTextBox.Text,
                    Password = accountNewPasswordTextBox.Text,
                    Name = username,
                    ConfirmCode = code,
                };
                Data.Entity.Stat stat = new Stat
                {
                    Id = Guid.NewGuid(),
                    Account_Id = account.Id.ToString(),
                    Weight = accountNewWeightTextBox.Text,
                    Height = accountNewHeightTextBox.Text,
                    Age = accountNewAgeTextBox.Text,
                    Max_Target = "3000",
                    Picture = null,
                    Target_Points = "0"
                    
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
                using SmtpClient? smtpClient = GetSmtpClient();
                smtpClient?.Send(
                    App.GetConfiguration("smtp:email")!,
                    accountNewMailTextBox.Text,
                    "Signup successfull",
                    $"Congratulations! To confirm Email use code: {code}"
                );
                MessageBox.Show("Chek Email");
            });
        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (accountPasswordTextBox.Text != "Password")
            {
                if (hiddenPassword)
                {
                    accountPasswordTextBox.Text = accountHidePasswordTextBox.Password;
                    UnhidePassword.Visibility = Visibility.Visible;
                    HidePassword.Visibility = Visibility.Hidden;
                    hiddenPassword = false;
                }
                else
                {
                    accountHidePasswordTextBox.Password = accountPasswordTextBox.Text;
                    UnhidePassword.Visibility = Visibility.Hidden;
                    HidePassword.Visibility = Visibility.Visible;
                    hiddenPassword = true;
                }
            }
        }

        private void confirmCodeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (confirmCodeTextBox.Text == "Confirm code")
            confirmCodeTextBox.Text = "";
        }

        private void confirmCodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (confirmCodeTextBox.Text == "")
                confirmCodeTextBox.Text = "Confirm code";
        }

        private void confirmCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            var confCode = dataContext.Accounts.FirstOrDefault(acc => acc.Mail == accountMailTextBox.Text);
            if (confCode.ConfirmCode == confirmCodeTextBox.Text)
            {
                MessageBox.Show("Email successfully confirmed", "Login information", MessageBoxButton.OK, MessageBoxImage.Information);
                confCode.ConfirmCode = null;
                dataContext.SaveChanges();
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
                String code = Guid.NewGuid().ToString()[..6].ToUpper();
                MessageBox.Show("Invalid code, check your email and try again", "Login information", MessageBoxButton.OK, MessageBoxImage.Error);
                using SmtpClient? smtpClient = GetSmtpClient();
                smtpClient?.Send(
                    App.GetConfiguration("smtp:email")!,
                    accountMailTextBox.Text,
                    "Signup successfull",
                    $"Congratulations! To confirm Email use code: {code}"
                );
                confCode.ConfirmCode = code;
                dataContext.SaveChanges();
            }
        }
        private SmtpClient? GetSmtpClient()
        {
            #region get and check config
            String? host = App.GetConfiguration("smtp:host");
            if (host == null)
            {
                MessageBox.Show("Error getting host");
                return null;
            }
            String? portString = App.GetConfiguration("smtp:port");
            if (portString == null)
            {
                MessageBox.Show("Error getting port");
                return null;
            }
            int port;
            try { port = int.Parse(portString); }
            catch
            {
                MessageBox.Show("Error parsing port");
                return null;
            }
            String? email = App.GetConfiguration("smtp:email");
            if (email == null)
            {
                MessageBox.Show("Error getting email");
                return null;
            }
            String? password = App.GetConfiguration("smtp:password");
            if (password == null)
            {
                MessageBox.Show("Error getting password");
                return null;
            }
            String? sslString = App.GetConfiguration("smtp:ssl");
            if (sslString == null)
            {
                MessageBox.Show("Error getting ssl");
                return null;
            }
            bool ssl;
            try { ssl = bool.Parse(sslString); }
            catch
            {
                MessageBox.Show("Error parsing ssl");
                return null;
            }
            #endregion

            return new(host, port)
            {
                EnableSsl = ssl,
                Credentials = new NetworkCredential(email, password)
            };
        }

        private void accountPasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (accountPasswordTextBox.Text == "Password")
                accountPasswordTextBox.Text = "";


        }

        private void accountPasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (accountPasswordTextBox.Text == "")
            {
                accountPasswordTextBox.Text = "Password";
            }
        }
        private void accountHidePasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (accountPasswordTextBox.Text == "Password")
            {
                accountPasswordTextBox.Text = "";
            }
        }
        private void accountHidePasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (accountHidePasswordTextBox.Password == "")
            {
                accountPasswordTextBox.Text = "Password";
                UnhidePassword.Visibility = Visibility.Visible;
                HidePassword.Visibility = Visibility.Hidden;
            }
        }

        private void accountMailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (accountMailTextBox.Text == "Email")
                accountMailTextBox.Text = "";
        }
        private void accountMailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (accountMailTextBox.Text == "")
                accountMailTextBox.Text = "Email";
        }

        private void accountPasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (accountPasswordTextBox.Text == "")
            {
                accountPasswordTextBox.Text = accountHidePasswordTextBox.Password;
                hiddenPassword = true;
                accountHidePasswordTextBox.Password = "";
                accountHidePasswordTextBox.Focus();
                UnhidePassword.Visibility = Visibility.Hidden;
                HidePassword.Visibility = Visibility.Visible;
            }
        }

    }
}
