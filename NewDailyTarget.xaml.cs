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
    /// Логика взаимодействия для NewDailyTarget.xaml
    /// </summary>
    public partial class NewDailyTarget : Window
    {
        Brain br = new Brain();
        public int daily_target;
        public NewDailyTarget()
        {
            InitializeComponent();
        }

        private void save_daily_target_btn_Click(object sender, RoutedEventArgs e)
        {
            int is_out = 1;
            try
            {
                daily_target = int.Parse(new_target_txt.Text);
                is_out = 1;
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter new target in numbers only");
                is_out = 0;
            }
            if (is_out == 1)
            {
                Close();
                br.WriteToFile(br.target_path,daily_target.ToString());
            }
        }
        public void SetBlackMode()
        {
            Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/background_dark.jpg", UriKind.Relative)));
            set_target_label.Background = Brushes.LightGray;
            set_target_label.Foreground = Brushes.Black;
            set_target_label.BorderBrush = Brushes.Black;

            new_target_txt.Background = Brushes.LightGray;
            new_target_txt.Foreground = Brushes.Black;
            new_target_txt.BorderBrush = Brushes.Black;

            save_daily_target_btn.Background = Brushes.LightGray;
            save_daily_target_btn.Foreground = Brushes.Black;
            save_daily_target_btn.BorderBrush = Brushes.Black;
        }
    }
}
