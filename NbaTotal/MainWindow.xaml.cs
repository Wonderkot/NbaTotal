using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using CalcLib.BusinessLogic.Implementation;
using CalcLib.BusinessLogic.Interfaces;
using CalcLib.Data;
using Newtonsoft.Json.Linq;


namespace NbaTotal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        readonly ICalc _calc = new Calc();

        public MainWindow()
        {
            InitializeComponent();
            SetTeamsNames(Team1Cb, TeamConsts.FullNames);
            SetTeamsNames(Team2Cb, TeamConsts.FullNames);
            _calc.ShowMessage += CalcOnShowMessage;
            _calc.SetShowMessage();
        }

        private void CalcOnShowMessage(string s)
        {
            //TODO пока так, потом может лог в отдельную вкладку сделаю
            MessageBox.Show(s);
        }

        private void CalcBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Team1Cb.SelectedValue == null || Team2Cb.SelectedValue == null)
            {
                MessageBox.Show("Значения для расчета не выбраны!");
                return;
            }
            long id1 = (long)Team1Cb.SelectedValue;
            long id2 = (long)Team2Cb.SelectedValue;
            CalcBtn.IsEnabled = false;
            Team1Name.Text = Team1Cb.Text;
            Team2Name.Text = Team2Cb.Text;
            Thread test = new Thread(() =>
            {
                SetResult(id1, id2);
            });
            test.Start();
        }

        private void SetResult(long team1, long team2)
        {
            var result = _calc.GetResultByTeam(team1, team2);
            if (result == null)
            {
                MessageBox.Show("Расчет невозможен, данные не получены!");
                return;
            }

            Result.Dispatcher.Invoke(() =>
            {
                Result.Text = result.Text;
            });

            SetSummary(result.Team1, Team1Summary);
            SetSummary(result.Team2, Team2Summary);
            CalcBtn.Dispatcher.Invoke(() =>
            {
                CalcBtn.IsEnabled = true;
            });
        }

        private void SetTeamsNames(ComboBox comboBox, Dictionary<long, string> teams)
        {
            comboBox.ItemsSource = teams;
            comboBox.DisplayMemberPath = "Value";
            comboBox.SelectedValuePath = "Key";
        }

        private void SetSummary(JObject teamData, TextBlock textBlock)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Забитые мячи: {teamData["Pts"]}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"Разность мячей : {teamData["PlusMinus"]}");
            textBlock.Dispatcher.Invoke(() =>
            {
                textBlock.Text = sb.ToString();
            });
        }

        private void ManualCalcBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
