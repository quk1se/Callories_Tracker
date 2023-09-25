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
            my_pict_path_txt = br.ReadFromFile(br.picture_path);
            br.SetStartAvatar(my_pict_path_txt, your_avatar);
            NameTextBlock.Text = br.ReadFromFile(br.file_path);
        }

        private void save_account_data_Click(object sender, RoutedEventArgs e)
        {
            your_name_txt = NameTextBlock.Text;
            br.WriteToFile(br.file_path,your_name_txt);
            if (int.TryParse(parameters_age.Text, out int age) &&
                int.TryParse(parameters_weight.Text, out int weight) &&
                int.TryParse(parameters_height.Text, out int height))
            {
                br.WriteToFile(br.age_path, age.ToString());
                br.WriteToFile(br.height_path, height.ToString());
                br.WriteToFile(br.weight_path, weight.ToString());
                Close();
            } 
            else 
            { 
                MessageBox.Show("Please, enter valid data", "Data error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void your_avatar_Click(object sender, RoutedEventArgs e)
        {
            my_pict_path_txt = br.TakePicturePath();
            your_avatar.Source = new BitmapImage(new Uri(my_pict_path_txt, UriKind.RelativeOrAbsolute));
            your_avatar.Stretch = Stretch.UniformToFill;
            br.WriteToFile(br.picture_path,my_pict_path_txt);
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
