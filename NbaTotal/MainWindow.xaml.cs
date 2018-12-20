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
        readonly  ICalc _calc = new Calc();
        public MainWindow()
        {
            InitializeComponent();
            Dictionary<long, string> teams = _dataProvider.GetAllTeams();
            SetData(Team1Cb, teams);
            SetData(Team2Cb, teams);
        }

        private void CalcBtn_OnClick(object sender, RoutedEventArgs e)
        {
            long id1 = (long) Team1Cb.SelectedValue;
            long id2 = (long) Team2Cb.SelectedValue;

            var x = _calc.GetResult(id1, id2);
            Result.Text = x.ToString();
            Team1Summary.Text = Team1Cb.Text;
            Team2Summary.Text = Team2Cb.Text;
        }

        private void SetData(ComboBox comboBox, Dictionary<long, string> teams)
        {
            comboBox.ItemsSource = teams;
            comboBox.DisplayMemberPath = "Value";
            comboBox.SelectedValuePath = "Key";
        }
    }
}
