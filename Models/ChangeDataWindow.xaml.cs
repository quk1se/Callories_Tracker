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
using System.IO;
using Microsoft.Win32;
using System.Drawing;

namespace Callories_Tracker
{
    /// <summary>
    /// Логика взаимодействия для ChangeDataWindow.xaml
    /// </summary>
    public partial class ChangeDataWindow : Window
    {
        Brain br = new Brain();
        public string your_name_txt;
        public string my_pict_path_txt;
        public string picture_start_path = "D:\\Prog_profile\\Callories_Tracker\\AccountData\\account_picture.txt";

        public string ParamAge { get; set; }
        public string ParamWeight { get; set; }
        public string ParamHeight { get; set; }


        public ChangeDataWindow()
        {
            InitializeComponent();
            br.SetStartAvatar(Brain.picture_path, your_avatar);
            NameTextBlock.Text = Brain.account_name;
            parameters_age.Text = Brain.account_age;
            parameters_weight.Text = Brain.account_weight;
            parameters_height.Text = Brain.account_height;
        }

        private void save_account_data_Click(object sender, RoutedEventArgs e)
        {
            Brain.account_name = NameTextBlock.Text;
            if (int.TryParse(parameters_age.Text, out int age) &&
                int.TryParse(parameters_weight.Text, out int weight) &&
                int.TryParse(parameters_height.Text, out int height))
            {
                Brain.account_age = parameters_age.Text;
                Brain.account_weight = parameters_weight.Text;
                Brain.account_height = parameters_height.Text;
                Brain.picture_path = my_pict_path_txt;
                Close();
            } 
            else 
            { 
                MessageBox.Show("Please, enter valid data", "Data error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void your_avatar_Click(object sender, RoutedEventArgs e)
        {
            string temp_path = Brain.picture_path;
            my_pict_path_txt = br.TakePicturePath();
            try
            {
                your_avatar.Source = new BitmapImage(new Uri(my_pict_path_txt, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                if (temp_path == null) return;
                your_avatar.Source = new BitmapImage(new Uri(temp_path, UriKind.RelativeOrAbsolute));
                my_pict_path_txt = temp_path;
            }
            your_avatar.Stretch = Stretch.UniformToFill;
        }

        public void SetBlackMode()
        {
            Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/background_dark.jpg", UriKind.Relative)));
            your_avatar_rect.Stroke = Brushes.Black;
            save_account_data.Background = Brushes.LightGray;
            save_account_data.Foreground = Brushes.Black;
            save_account_data_rect.Stroke = Brushes.Black;
        }
    }
}
