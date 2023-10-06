using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Security.Policy;
using System.Windows.Shapes;
using System.Windows.Documents;
using OxyPlot.Wpf;

namespace Callories_Tracker
{
    public class Quote
    {
        public string text { get; set; }
        public string author { get; set; }
    }

    class Brain
    {
        public static string account_name;
        public static string account_age;
        public static string account_weight;
        public static string account_height;
        public static string picture_path;
        public static string daily_max_target;

        public string[] advice_nums = { "1/3", "2/3", "3/3" };
        public string advice;
        public bool human_parameters_visible = false;
        public bool notifications_switch = false;
        public bool advice_switch = false;
        public bool dark_mode = false;
        public float target_points = 0;
        public float part_of_target;
        public string no_avatar_path = "D:\\Prog_profile\\Callories_Tracker\\AppStyle\\no_avatar.jpg";
        public string target_path = "D:\\Prog_profile\\Callories_Tracker\\AccountData\\daily_target.txt";
        public Dictionary<Button,Boolean> achievements;
        public List<Button> plus_buttons_list;
        public List<Polygon> triangle_polygons;
        public List<Button> advice_buttons_list;
        public List<Rectangle> advice_rects_list;
        public List<Button> panel_buttons_list;
        public List<Rectangle> panel_rectangle_list;
        public List<Button> options_buttons_list;
        public List<Rectangle> options_rectangle_list;
        public List<Button> profile_buttons_list;
        public List<Rectangle> profile_rectangle_list;
        public Brain()
        {
            
        }

        public async Task<string[]> GetMotivationalQuotesAsync()
        {
            string apiUrl = "https://type.fit/api/quotes";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        var quotes = Newtonsoft.Json.JsonConvert.DeserializeObject<Quote[]>(jsonContent);

                        List<string> adviceList = new List<string>();
                        Random random = new Random();
                        for (int i = 0; i < 3; i++)
                        {
                            int index = random.Next(0, quotes.Length);
                            string advice = quotes[index].text;
                            adviceList.Add(advice);
                        }

                        return adviceList.ToArray();
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                return new string[]
                {
                "Цитата 1",
                "Цитата 2",
                "Цитата 3"
                };
            }
        }
        public void GridVisibleChanged(Grid gr1,Grid gr2,Grid gr3, Grid gr4, Grid gr5, Grid gr6)
        {
            gr1.Visibility = Visibility.Visible;
            gr2.Visibility = Visibility.Hidden;
            gr3.Visibility = Visibility.Hidden;
            gr4.Visibility = Visibility.Hidden;
            gr5.Visibility = Visibility.Hidden;
            gr6.Visibility = Visibility.Hidden;
        }
        public bool NotificationsSwitch()
        {
            if (notifications_switch == false) notifications_switch = true;
            else notifications_switch = false;
            return notifications_switch;
        }
        public bool AdviceSwitch()
        {
            if (advice_switch == false) advice_switch = true;
            else advice_switch = false;
            return advice_switch;
        }
        public void WriteToFile(string filePath, string dataToWrite)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(dataToWrite);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи в файл: " + ex.Message);
            }
        }
        public string ReadFromFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string data = reader.ReadToEnd();
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка чтения файла: " + ex.Message);
                return null;
            }
        }
        public string TakePicturePath()
        {
            string temp_path = ReadFromFile(picture_path);
            string path = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    path = System.IO.Path.GetFullPath(openFileDialog.FileName);
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Error, incorrect image file format!");
                    path = temp_path;
                    return path;
                }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("Error, pick the image!");
                    path = temp_path;
                    return path;
                }
                
            }
            if (path == "")
            {
                return temp_path;
            }
            else
                return path;
        }
        public void SetStartAvatar(string path,Image img)
        {   
            if (ReadFromFile(path) == null)
            {
                img.Source = new BitmapImage(new Uri(no_avatar_path, UriKind.RelativeOrAbsolute));
                img.Stretch = Stretch.UniformToFill;
            }
            else
            {
                img.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                img.Stretch = Stretch.UniformToFill;
            }
        }
        public void SetPartsTarget()
        {
            part_of_target = Int32.Parse(daily_max_target) / 8;
        }
        public void FillRect(Polygon rect)
        {
            Color color;
            if (dark_mode) color = Colors.Black;
            else color = Colors.DarkOliveGreen;
            SolidColorBrush brush = new SolidColorBrush(color);
            rect.Fill = brush;
        }
        public void FillRectWhite(Polygon rect)
        {
            Color color;
            if (dark_mode) color = Colors.LightGray;
            else color = Colors.FloralWhite;
            SolidColorBrush brush = new SolidColorBrush(color);
            rect.Fill = brush;
        }
        public static Style GetCustomButtonStyle()
        {
            return Application.Current.FindResource("DarkModeBtn") as Style;
        }
        public void CheckPanelBtnStyle(Button btn1, Button btn2, Button btn3, Button btn4, Button btn5, Style dark_style, Style light_style)
        {
            if (dark_mode)
            {
                btn1.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/achievement_dark.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                btn2.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/panel_avatar_icn_dark.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                btn3.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/target_dark.jpg"))) { Stretch = Stretch.UniformToFill };
                btn4.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/advice_dark.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                btn5.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/options_dark.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                foreach (var item in panel_buttons_list) 
                {
                    item.Style = dark_style;
                }
            }
            else if (!dark_mode)
            {
                btn1.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/achievement.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                btn2.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/panel_avatar_icn.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                btn3.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/target.jpg"))) { Stretch = Stretch.UniformToFill };
                btn4.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/advice.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                btn5.Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/options.jpg", UriKind.Relative))) { Stretch = Stretch.UniformToFill };
                foreach (var item in panel_buttons_list)
                {
                    item.Style = light_style;
                }
            }
        }
        public void CheckPanelRectStyle(Rectangle rect1, Style dark_style, Style light_style)
        {
            if (dark_mode)
            {
                rect1.Fill = Brushes.LightGray;
                rect1.Stroke = Brushes.Black;
                foreach (var item in panel_rectangle_list)
                {
                    item.Style = dark_style;
                }
            }
            else if (!dark_mode)
            {
                rect1.Fill = Brushes.FloralWhite;
                rect1.Stroke = Brushes.DarkOliveGreen;
                foreach (var item in panel_rectangle_list)
                {
                    item.Style = light_style;
                }
            }
        }
        public void CheckOptionsRectStyle(Style dark_style, Style light_style)
        {
            if (dark_mode)
            {
                foreach (var item in options_rectangle_list)
                {
                    item.Style = dark_style;
                }
            }
            else if (!dark_mode)
            {
                foreach (var item in options_rectangle_list)
                {
                    item.Style = light_style;
                }
            }
        }
        public void CheckOptionsBtnStyle(Style dark_style, Style light_style)
        {
            if (dark_mode)
            {
                foreach (var item in options_buttons_list)
                {
                    item.Style = dark_style;
                }
            }
            else if (!dark_mode)
            {
                foreach (var item in options_buttons_list)
                {
                    item.Style = light_style;
                }
            }
        }

        //====================================ACHIEVE====================================
        public void CheckAchieveStyle(Style light_style_complete, Style dark_style_complete, Style light_style_not_complete, Style dark_style_not_complete)
        {
            if (dark_mode)
            {
                foreach (var item in achievements)
                {
                    if (item.Value) item.Key.Style = dark_style_complete;
                    else if (!item.Value) item.Key.Style = dark_style_not_complete;
                }
            }

            else if (!dark_mode)
            {
                foreach (var item in achievements)
                {
                    if (item.Value) item.Key.Style = light_style_complete;
                    else if (!item.Value) item.Key.Style = light_style_not_complete;
                }
            }
        }

        //====================================PLUSBTN====================================
        public void CheckPlusBtnsStyle(Style dark_rect_style, Style light_rect_style, Style light_style, Style dark_style, Label label, Rectangle rect)
        {
            if (dark_mode)
            {
                label.Foreground = Brushes.Black;
                rect.Style = dark_rect_style;
                foreach (var item in plus_buttons_list) item.Style = dark_style;
            }
            else if (!dark_mode)
            {
                label.Foreground = Brushes.DarkOliveGreen;
                rect.Style = light_rect_style;
                foreach (var item in plus_buttons_list) item.Style = light_style;
            }
        }
            public void CheckTrianglePolygonsStyle(Style light_style, Style dark_style)
        {
            if (dark_mode) foreach (var item in triangle_polygons) item.Style = dark_style;
            else if (!dark_mode) foreach (var item in triangle_polygons) item.Style = light_style;
        }
        public void CheckAdviceBtnsAndRectsStyle(Style light_style_btn, Style dark_style_btn, Style light_style_rect, Style dark_style_rect)
        {
            if (dark_mode)
            {
                foreach (var item in advice_buttons_list) item.Style = dark_style_btn;
                foreach (var item in advice_rects_list) item.Style = dark_style_rect;
            }
            else if (!dark_mode)
            {
                foreach (var item in advice_buttons_list) item.Style = light_style_btn;
                foreach (var item in advice_rects_list) item.Style = light_style_rect;
            }
        }
        public void CheckAdviceTextStyle(Rectangle rect, Style dark_rect_style, Style light_rect_style,Paragraph par, Label label)
        {
            if (dark_mode)
            {
                rect.Style = dark_rect_style;
                par.Foreground = Brushes.Black;
                label.Foreground = Brushes.Black;
            }
            else if (!dark_mode)
            {
                rect.Style = light_rect_style;
                par.Foreground = Brushes.DarkOliveGreen;
                label.Foreground = Brushes.DarkOliveGreen;
            }
        }
        public void CheckProfileStyle(Style dark_rect_style, Style light_rect_style, Style dark_btn_style, Style light_btn_style, PlotView pl1, PlotView pl2)
        {
            if (dark_mode)
            {
                pl1.Background = Brushes.LightGray;
                pl2.Background = Brushes.LightGray;
                foreach (var item in profile_rectangle_list)
                {
                    item.Style = dark_rect_style;
                }
                foreach (var item in profile_buttons_list)
                {
                    item.Style = dark_btn_style;
                }
            }
            else if (!dark_mode)
            {
                pl1.Background = Brushes.FloralWhite;
                pl2.Background = Brushes.FloralWhite;
                foreach (var item in profile_rectangle_list)
                {
                    item.Style = light_rect_style;
                }
                foreach (var item in profile_buttons_list)
                {
                    item.Style = light_btn_style;
                }
            }
        }
    }
}

