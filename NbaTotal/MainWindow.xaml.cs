using System.Windows;
using CalcLib.BusinessLogic.Implementation;
using CalcLib.BusinessLogic.Interfaces;


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
            var teams = _dataProvider.GetAllTeams();
        }

        private void CalcBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Team1Summary.Text = "Team1";
            Team2Summary.Text = "Team2";
        }
    }
}
