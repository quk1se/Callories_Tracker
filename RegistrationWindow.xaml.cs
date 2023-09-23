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
        Brain brain = new Brain();
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void signInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (accountMailTextBox.Text == brain.ReadFromFile(brain.file_path) && accountPasswordTextBox.Text == brain.ReadFromFile(brain.pass_path))
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
    }
}
