using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Threading;
using Callories_Tracker.Data;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Identity.Client;
using System.Collections;

namespace Callories_Tracker
{
    public partial class MainWindow : Window
    {
        private DataContext dataContext;
        Brain brain = new Brain();
        public string[] advice;
        private int clickCount = 0;
        private DispatcherTimer timer;
       

        public async Task TakeAdvices()
        {
            advice = await brain.GetMotivationalQuotesAsync();
            advice_text.Inlines.Add(advice[0]);
        }
        public MainWindow()
        {
            InitializeComponent();
            dataContext = new();
            brain.human_parameters_visible = true;
            GetUserData();


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick!;
           
            brain.plus_buttons_list = new List<Button> {plus_one_btn, plus_two_btn, plus_five_btn, plus_ten_btn, plus_twofive_btn, plus_fifty_btn,
                                                        plus_one_hundread_btn, plus_two_hundread_btn, plus_five_hundread_btn };
            brain.triangle_polygons = new List<Polygon> {target_triangle, first_fill, second_fill, third_fill, fourth_fill, fifth_fill,
                                                        six_fill, seven_fill, eight_fill };
            brain.advice_buttons_list = new List<Button> { next_advice_btn, previous_advice_btn };
            brain.advice_rects_list = new List<Rectangle> { next_advice_rect, previous_advice_rect };
            brain.panel_buttons_list = new List<Button> { achievement_btn, profile_btn, target_btn, daily_advice_btn, options_btn };
            brain.panel_rectangle_list = new List<Rectangle> { achievement_rect, profile_rect, target_rect, daily_advice_rect, options_rect };
            brain.options_buttons_list = new List<Button> { set_new_target_btn, change_account_data_btn, notifications_switch_btn, advice_switch_btn, dark_mode_btn, log_out_btn };
            brain.options_rectangle_list = new List<Rectangle> { set_new_target_rect, change_account_data_rect, notifications_switch_rect, advice_switch_rect, dark_mode_rect, sign_in_rect };
            brain.profile_buttons_list = new List<Button> { weight_diagram_btn, circle_diagram_btn, account_parameters_btn };
            brain.profile_rectangle_list = new List<Rectangle> { avatar_rectangle, diagrams_rect };
            TakeAdvices();
            brain.SetPartsTarget();


            var model = new PlotModel { Title = "Weight statistic" };
            var series = new LineSeries
            {
                Color = OxyColors.Black, 
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Black,
                MarkerFill = OxyColors.Black
            };

            series.Points.Add(new DataPoint(0, 0));
            series.Points.Add(new DataPoint(1, 4));
            series.Points.Add(new DataPoint(2, 2));
            series.Points.Add(new DataPoint(3, 7));
            series.Points.Add(new DataPoint(30, 15));

            model.Series.Add(series);

            WeightDiagram.Model = model;

            Random random = new Random();
            var circle_model = new PlotModel { Title = "Круговая диаграмма" };

            var pieSeries = new PieSeries
            {
                StrokeThickness = 4, 
                InsideLabelPosition = 0.5, 
                AngleSpan = 360, 
                StartAngle = 0,
                InsideLabelColor = OxyColors.White,
                FontSize = 15, 
                FontWeight = OxyPlot.FontWeights.Bold,
                Stroke = OxyColors.Black,
            };

            pieSeries.Slices.Add(new PieSlice("Белки", 30) { Fill = OxyColor.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)) });
            pieSeries.Slices.Add(new PieSlice("Жиры", 45) { Fill = OxyColor.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)) });
            pieSeries.Slices.Add(new PieSlice("Углеводы", 25) { Fill = OxyColor.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)) });

            circle_model.Series.Add(pieSeries);

            CircleDiagram.Model = circle_model;
        }

        private void GetUserData()
        {
            your_name_field.Content = dataContext
                .Accounts
                .Where(acc => acc.Id.ToString() == RegistrationWindow.my_id)
                .Select(account => account.Name)
                .FirstOrDefault();
            parameters_weight.Content = dataContext
               .Stats
               .Where(stats => stats.Account_Id == RegistrationWindow.my_id)
               .Select(account => account.Weight)
               .FirstOrDefault();
            parameters_age.Content = dataContext
               .Stats
               .Where(stats => stats.Account_Id == RegistrationWindow.my_id)
               .Select(account => account.Age)
               .FirstOrDefault();
            parameters_height.Content = dataContext
               .Stats
               .Where(stats => stats.Account_Id == RegistrationWindow.my_id)
               .Select(account => account.Height)
               .FirstOrDefault();

            Brain.picture_path = dataContext
                .Stats
                .Where(stats => stats.Account_Id == RegistrationWindow.my_id)
                .Select(account => account.Picture)
                .FirstOrDefault()!;
            Brain.daily_max_target = dataContext
                .Stats
                .Where(stats => stats.Account_Id == RegistrationWindow.my_id)
                .Select(account => account.Max_Target)
                .FirstOrDefault()!;
            brain.SetStartAvatar(Brain.picture_path, your_avatar);
            Brain.account_name = your_name_field.Content!.ToString()!;
            Brain.account_age = parameters_age.Content!.ToString()!;
            Brain.account_weight = parameters_weight.Content!.ToString()!;
            Brain.account_height = parameters_height.Content!.ToString()!;
            SetAchievementsButtons();
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            if (clickCount >= 10)
            {
                var achievUpdate = dataContext.Achievements.FirstOrDefault(achiev => achiev.AccountId == RegistrationWindow.my_id);
                epilepsy.Content = "Epilepsy";
                if (brain.achievements[epilepsy] == "false" && brain.dark_mode)
                {
                    Style dark_complete_achieve_style = (Style)FindResource("AchievementCompletedDark");
                    epilepsy.Style = dark_complete_achieve_style;
                }
                else if (brain.achievements[epilepsy] == "false" && !brain.dark_mode)
                {
                    Style light_complete_achieve_style = (Style)FindResource("AchievementCompletedLight");
                    epilepsy.Style = light_complete_achieve_style;
                }
                achievUpdate!.Epilepsy = "true";
                dataContext.SaveChanges();
                clickCount = 0;

            }
        }

        private void weight_diagram_btn_Click(object sender, RoutedEventArgs e)
        {
            HumanParametersGrid.Visibility = Visibility.Hidden;
            CircleDiagram.Visibility = Visibility.Hidden;
            WeightDiagram.Visibility = Visibility.Visible;
            brain.human_parameters_visible = false;
        }

        private void circle_diagram_btn_Click(object sender, RoutedEventArgs e)
        {
            HumanParametersGrid.Visibility = Visibility.Hidden;
            WeightDiagram.Visibility = Visibility.Hidden;
            CircleDiagram.Visibility = Visibility.Visible;
            brain.human_parameters_visible = false;
        }
        private void account_parameters_btn_Click(object sender, RoutedEventArgs e)
        {
            CircleDiagram.Visibility = Visibility.Hidden;
            WeightDiagram.Visibility = Visibility.Hidden;
            HumanParametersGrid.Visibility = Visibility.Visible;
            brain.human_parameters_visible = true;
        }

        private void profile_btn_Click(object sender, RoutedEventArgs e)
        {
            brain.GridVisibleChanged(ProfileGrid,OptionsGrid,TargetGrid, DailyAdviceGrid, AchievementsGrid, HumanParametersGrid);
            if (brain.human_parameters_visible == true) HumanParametersGrid.Visibility = Visibility.Visible;
            your_name_field.Content = Brain.account_name;
            parameters_age.Content = Brain.account_age;
            parameters_weight.Content = Brain.account_weight;
            parameters_height.Content = Brain.account_height;
            try
            {
                your_avatar.Source = new BitmapImage(new Uri(Brain.picture_path, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                your_avatar.Source = new BitmapImage(new Uri(brain.no_avatar_path, UriKind.RelativeOrAbsolute));
            }
            your_avatar.Stretch = Stretch.UniformToFill;
        }
        private void target_btn_Click(object sender, RoutedEventArgs e)
        {
            brain.GridVisibleChanged(TargetGrid, OptionsGrid, ProfileGrid, DailyAdviceGrid, AchievementsGrid, HumanParametersGrid);
            target_progress.Content = $"{brain.target_points}/{Brain.daily_max_target}";
            brain.part_of_target = Int32.Parse(Brain.daily_max_target) / 8;

            if (brain.target_points >= brain.part_of_target * 8) brain.FillRect(eight_fill);
            else brain.FillRectWhite(eight_fill);
            if (brain.target_points >= brain.part_of_target * 7) brain.FillRect(seven_fill);
            else brain.FillRectWhite(seven_fill);
            if (brain.target_points >= brain.part_of_target * 6) brain.FillRect(six_fill);
            else brain.FillRectWhite(six_fill);
            if (brain.target_points >= brain.part_of_target * 5) brain.FillRect(fifth_fill);
            else brain.FillRectWhite(fifth_fill);
            if (brain.target_points >= brain.part_of_target * 4) brain.FillRect(fourth_fill);
            else brain.FillRectWhite(fourth_fill);
            if (brain.target_points >= brain.part_of_target * 3) brain.FillRect(third_fill);
            else brain.FillRectWhite(third_fill);
            if (brain.target_points >= brain.part_of_target * 2) brain.FillRect(second_fill);
            else brain.FillRectWhite(second_fill);
            if (brain.target_points >= brain.part_of_target) brain.FillRect(first_fill);
            else brain.FillRectWhite(first_fill);
        }

        private void options_btn_Click(object sender, RoutedEventArgs e)
        {
            brain.GridVisibleChanged(OptionsGrid, ProfileGrid, TargetGrid, DailyAdviceGrid,AchievementsGrid, HumanParametersGrid);
        }
        private void daily_advice_btn_Click(object sender, RoutedEventArgs e)
        {
            brain.GridVisibleChanged(DailyAdviceGrid, OptionsGrid, TargetGrid, ProfileGrid, AchievementsGrid, HumanParametersGrid);
        }
        private void change_account_data_btn_Click(object sender, RoutedEventArgs e)
        {
            ChangeDataWindow changeDataWindow = new ChangeDataWindow();
            if (brain.dark_mode) changeDataWindow.SetBlackMode();
            this.Hide();
            changeDataWindow.ShowDialog();
            this.Show();
        }
        private void achievement_btn_Click(object sender, RoutedEventArgs e)
        {
            brain.GridVisibleChanged(AchievementsGrid, OptionsGrid, TargetGrid, ProfileGrid, DailyAdviceGrid, HumanParametersGrid);
        }

        private void previous_advice_btn_Click(object sender, RoutedEventArgs e)
        {
            switch (advice_number.Content)
            {
                case "1/3":
                    break;
                case "2/3":
                    advice_number.Content = brain.advice_nums[0];
                    advice_text.Inlines.Clear();
                    advice_text.Inlines.Add(advice[0]);
                    break;
                case "3/3":
                    advice_number.Content = brain.advice_nums[1];
                    advice_text.Inlines.Clear();
                    advice_text.Inlines.Add(advice[1]);
                    break;
            }
        }

        private void next_advice_btn_Click(object sender, RoutedEventArgs e)
        {
            switch (advice_number.Content)
            {
                case "1/3":
                    advice_number.Content = brain.advice_nums[1];
                    advice_text.Inlines.Clear();
                    advice_text.Inlines.Add(advice[1]);
                    break;
                case "2/3":
                    advice_number.Content = brain.advice_nums[2];
                    advice_text.Inlines.Clear();
                    advice_text.Inlines.Add(advice[2]);
                    break;
                case "3/3":
                    break;
            }
        }

        private void notifications_switch_btn_Click(object sender, RoutedEventArgs e)
        {
            if (brain.NotificationsSwitch() == true) notifications_switch_btn.Content = "On notifications";
            else notifications_switch_btn.Content = "Off notifications";

            
        }

        private void advice_switch_btn_Click(object sender, RoutedEventArgs e)
        {
            if (brain.AdviceSwitch() == true) advice_switch_btn.Content = "On daily advice";
            else advice_switch_btn.Content = "Off daily advice";
        }
        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button)
            {
                brain.target_points += int.Parse(button.Content.ToString());
                target_progress.Content = $"{brain.target_points}/{Brain.daily_max_target}";
            }

            if (brain.target_points >= brain.part_of_target * 8) brain.FillRect(eight_fill);
            if (brain.target_points >= brain.part_of_target * 7) brain.FillRect(seven_fill);
            if (brain.target_points >= brain.part_of_target * 6) brain.FillRect(six_fill);
            if (brain.target_points >= brain.part_of_target * 5) brain.FillRect(fifth_fill);
            if (brain.target_points >= brain.part_of_target * 4) brain.FillRect(fourth_fill);
            if (brain.target_points >= brain.part_of_target * 3) brain.FillRect(third_fill);
            if (brain.target_points >= brain.part_of_target * 2) brain.FillRect(second_fill);
            if (brain.target_points >= brain.part_of_target) brain.FillRect(first_fill);
        }

        private void set_new_target_btn_Click(object sender, RoutedEventArgs e)
        {
            NewDailyTarget newdt = new NewDailyTarget();
            if (brain.dark_mode) newdt.SetBlackMode();
            newdt.ShowDialog();
        }

        private void dark_mode_btn_Click(object sender, RoutedEventArgs e)
        {
            clickCount++;
            if (brain.dark_mode)
            {
                dark_mode_btn.Content = "Dark mode";
                brain.dark_mode = false;
                Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/background.jpg", UriKind.Relative)));
            }
            else if (!brain.dark_mode)
            {
                dark_mode_btn.Content = "Light mode";
                brain.dark_mode = true;
                Background = new ImageBrush(new BitmapImage(new Uri("D:/Prog_profile/Callories_Tracker/AppStyle/background_dark.jpg", UriKind.Relative)));
            }
            if (!timer.IsEnabled)
            {
                timer.Start();
            }
            //==============STYLES DARK==============
            Style dark_option_btn_style = (Style)FindResource("DarkOptionsBtn");
            Style dark_option_rect_style = (Style)FindResource("DarkOptionsRect");
            Style dark_diagram_btn_style = (Style)FindResource("DarkDiagramButtonStyle");
            Style dark_panel_btn_style = (Style)FindResource("DarkPanelButton");
            Style dark_panel_rect_style = (Style)FindResource("DarkPanelRect");
            Style dark_plus_btn_style = (Style)FindResource("DarkPlusButton");
            Style dark_advice_btn_style = (Style)FindResource("DarkAdviceButton");
            Style dark_advice_rect_style = (Style)FindResource("DarkAdviceRect");
            Style dark_polygon_style = (Style)FindResource("PolygonDark");
            Style dark_target_rect_style = (Style)FindResource("TargetRectDark");
            Style dark_profile_rect_style = (Style)FindResource("ProfileRectangleDark");
            Style dark_not_complete_achieve_style = (Style)FindResource("AchievementNotCompletedDark");
            Style dark_complete_achieve_style = (Style)FindResource("AchievementCompletedDark");
            //==============STYLES LIGHT==============
            Style light_option_btn_style = (Style)FindResource("LightOptionsButton");
            Style light_option_rect_style = (Style)FindResource("LightOptionsRectangle");
            Style light_diagram_btn_style = (Style)FindResource("LightDiagramButtonStyle");
            Style light_panel_btn_style = (Style)FindResource("LightPanelButton");
            Style light_panel_rect_style = (Style)FindResource("LightPanelRect");
            Style light_plus_btn_style = (Style)FindResource("LightPlusButton");
            Style light_advice_btn_style = (Style)FindResource("LightAdviceButton");
            Style light_advice_rect_style = (Style)FindResource("LightAdviceRect");
            Style light_polygon_style = (Style)FindResource("PolygonLight");
            Style light_target_rect_style = (Style)FindResource("TargetRectLight");
            Style light_profile_rect_style = (Style)FindResource("ProfileRectangleLight");
            Style light_not_complete_achieve_style = (Style)FindResource("AchievementNotCompletedLight");
            Style light_complete_achieve_style = (Style)FindResource("AchievementCompletedLight");
            //==============================OPTIONS==============================
            brain.CheckOptionsBtnStyle(dark_option_btn_style,light_option_btn_style);
            brain.CheckOptionsRectStyle(dark_option_rect_style,light_option_rect_style);
            //==============================PANEL==============================
            brain.CheckPanelRectStyle(panel_rect, dark_panel_rect_style, light_panel_rect_style);
            brain.CheckPanelBtnStyle(achievement_btn, profile_btn, target_btn, daily_advice_btn, options_btn, dark_panel_btn_style, light_panel_btn_style);
            //==============================TARGET==============================
            brain.CheckPlusBtnsStyle(dark_target_rect_style, light_target_rect_style, light_plus_btn_style, dark_plus_btn_style,target_progress,target_progress_rect);
            brain.CheckTrianglePolygonsStyle(light_polygon_style, dark_polygon_style);
            //==============================ADVICE==============================
            brain.CheckAdviceTextStyle(advice_text_rectangle, dark_target_rect_style, light_target_rect_style,advice_text,advice_number);
            brain.CheckAdviceBtnsAndRectsStyle(light_advice_btn_style,dark_advice_btn_style,light_advice_rect_style,dark_advice_rect_style);
            //==============================PROFILE==============================
            brain.CheckProfileStyle(dark_profile_rect_style, light_profile_rect_style, dark_diagram_btn_style, light_diagram_btn_style, WeightDiagram, CircleDiagram);
            //==============================ACHIEVEMENT==============================
            brain.CheckAchieveStyle(light_complete_achieve_style, dark_complete_achieve_style, light_not_complete_achieve_style, dark_not_complete_achieve_style);
        }
        private void log_out_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out of your account?", "", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                this.Hide();
                new RegistrationWindow().ShowDialog();
                this.Close();
            }
        }
        private void SetAchievementsButtons()
        {
            brain.achievements = new Dictionary<Button, string> { 
                { start_of_a_long_journey,dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.StartOfALongJourney)
                .FirstOrDefault()!},
                { first_target,dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.FirstTarget)
                .FirstOrDefault()! },
                { streak_of_three,dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.StreakOfThree)
                .FirstOrDefault()! },
                { streak_of_five,dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.StreakOfFive)
                .FirstOrDefault()! },
                { streak_of_ten,dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.StreakOfTen)
                .FirstOrDefault()! },
                { complete_your_profile,dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.CompleteYourProfile)
                .FirstOrDefault()! },
                { lover_of_motivation, dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.LoverOfMotivation)
                .FirstOrDefault()! },
                { double_portion, dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.DoublePortion)
                .FirstOrDefault()! }, 
                { triple_portion,dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.TriplePortion)
                .FirstOrDefault()! },
                { accuracy_to_the_millimeter, dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.AccuracyToTheMillimeter)
                .FirstOrDefault()! }, 
                { leave_me_alone, dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.LeaveMeAlone)
                .FirstOrDefault()! },
                { epilepsy, dataContext.Achievements.Where(acc => acc.AccountId == RegistrationWindow.my_id)
                .Select(ach => ach.Epilepsy)
                .FirstOrDefault()! } };
            Style dark_not_complete_achieve_style = (Style)FindResource("AchievementNotCompletedDark");
            Style dark_complete_achieve_style = (Style)FindResource("AchievementCompletedDark");
            Style light_not_complete_achieve_style = (Style)FindResource("AchievementNotCompletedLight");
            Style light_complete_achieve_style = (Style)FindResource("AchievementCompletedLight");
            brain.CheckAchieveStyle(light_complete_achieve_style, dark_complete_achieve_style, light_not_complete_achieve_style, dark_not_complete_achieve_style);
        }

        private void CheckAchieveInfo(object sender, RoutedEventArgs e)
        {
            if (brain.achievements[(Button)sender] == "true")
            {
                if (sender == start_of_a_long_journey) MessageBox.Show("Register your account in the application and log in to it", start_of_a_long_journey.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == first_target) MessageBox.Show("Complete your first daily quota", first_target.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == streak_of_three) MessageBox.Show("Complete your daily requirement 3 days in a row", streak_of_three.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == streak_of_five) MessageBox.Show("Complete your daily requirement 5 days in a row", streak_of_five.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == streak_of_ten) MessageBox.Show("Complete your daily requirement 10 days in a row", streak_of_ten.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == complete_your_profile) MessageBox.Show("Fill out your profile completely", complete_your_profile.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == lover_of_motivation) MessageBox.Show("Read all the motivations for 5 days", lover_of_motivation.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == double_portion) MessageBox.Show("Exceed the daily norm by 2 times", double_portion.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == triple_portion) MessageBox.Show("Exceed the daily norm by 3 times", triple_portion.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == accuracy_to_the_millimeter) MessageBox.Show("Meet the daily norm exactly without overweights", accuracy_to_the_millimeter.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == leave_me_alone) MessageBox.Show("Keep notifications off for 5 days", leave_me_alone.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
                else if (sender == epilepsy) MessageBox.Show("Quickly change between dark and light mode within 5 seconds", epilepsy.Content.ToString(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
