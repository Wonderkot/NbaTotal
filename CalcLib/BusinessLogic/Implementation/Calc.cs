using System;
using System.Text;
using CalcLib.BusinessLogic.Interfaces;
using Newtonsoft.Json.Linq;

namespace CalcLib.BusinessLogic.Implementation
{
    public class Calc : ICalc
    {
        readonly IDataProvider _dataProvider = new DataProvider();
        public dynamic GetResult(long id1, long id2)
        {
            var team1 = _dataProvider.GetTeam(id1);
            var team2 = _dataProvider.GetTeam(id2);

            var sumPts = team1.Pts + team2.Pts;
            var sumPlusMinus = team1.PlusMinus + team2.PlusMinus;
            var ratio = (100 - sumPlusMinus) / 100;
            var result = Math.Round(ratio * sumPts);
            string totalOverUnder = ratio < 1 ? "ТБ" : "ТМ";
            if (team1.PlusMinus < 0 || team2.PlusMinus < 0)
            {
                totalOverUnder = "ТБ <-> ТМ";
            }
            var correction = Math.Abs(team1.PlusMinus) > Math.Abs(team2.PlusMinus) ? team1.PlusMinus : team2.PlusMinus;
            correction = Math.Round(correction);

            dynamic summary = new JObject();
            summary.Team1 = new JObject();
            summary.Team1.Pts = team1.Pts;
            summary.Team1.PlusMinus = team1.PlusMinus;

            summary.Team2 = new JObject();
            summary.Team2.Pts = team2.Pts;
            summary.Team2.PlusMinus = team2.PlusMinus;

            summary.SumPts = sumPts;
            summary.Ratio = ratio;
            summary.Result = string.Concat(totalOverUnder, " ", result.ToString());

            var sb = new StringBuilder();
            sb.AppendLine($"Сумма мячей: {sumPts}");
            //sb.AppendLine(string.Empty);
            sb.AppendLine($"Понижающий кф.: {ratio}");
            //sb.AppendLine(string.Empty);
            sb.AppendLine($"Прогноз: {string.Concat(totalOverUnder, " ", result.ToString())}");
            sb.AppendLine($"Корректирующий отступ: {correction}");
            sb.AppendLine($"Прогноз с учетом отступа: {string.Concat(totalOverUnder, " ", (result - correction).ToString())}");

            summary.Text = sb.ToString();

            return summary;
        }
    }
}