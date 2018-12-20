using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CalcLib.BusinessLogic.Implementation;
using CalcLib.BusinessLogic.Interfaces;
using CalcLib.Data;


namespace NbaTotal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly IDataProvider _dataProvider = new DataProvider();
        public MainWindow()
        {
            InitializeComponent();
            List<Team> teams = _dataProvider.GetAllTeams();
            SetData(Team1Cb, teams);
            SetData(Team2Cb, teams);
        }

        private void CalcBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Team1Summary.Text = "Team1";
            Team2Summary.Text = "Team2";
        }

        private void SetData(ComboBox comboBox, List<Team> teams)
        {
            comboBox.ItemsSource = teams;
            comboBox.DisplayMemberPath = "Abbreviation";
            comboBox.SelectedValuePath = "Id";
        }
    }
}
