using System.Collections.Generic;
using System.Text;
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
    public partial class MainWindow : Window
    {
        readonly IDataProvider _dataProvider = new DataProvider();
        readonly ICalc _calc = new Calc();
        public MainWindow()
        {
            InitializeComponent();
            //Dictionary<long, string> teams = _dataProvider.GetAllTeams();
            SetTeamsNames(Team1Cb, TeamConsts.FullNames);
            SetTeamsNames(Team2Cb, TeamConsts.FullNames);
        }

        private void CalcBtn_OnClick(object sender, RoutedEventArgs e)
        {
            long id1 = (long)Team1Cb.SelectedValue;
            long id2 = (long)Team2Cb.SelectedValue;

            var result = _calc.GetResult(id1, id2);

            Result.Text = result.Text;

            Team1Name.Text = Team1Cb.Text;
            Team2Name.Text = Team2Cb.Text;

            
            SetSummary(result.Team1, Team1Summary);
            SetSummary(result.Team2, Team2Summary);

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
            textBlock.Text = sb.ToString();
        }
    }
}
