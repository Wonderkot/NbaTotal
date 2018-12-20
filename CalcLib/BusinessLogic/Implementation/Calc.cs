using System;
using CalcLib.BusinessLogic.Interfaces;

namespace CalcLib.BusinessLogic.Implementation
{
    public class Calc : ICalc
    {
        readonly IDataProvider _dataProvider = new DataProvider();
        public object GetResult(long id1, long id2)
        {
            var team1 = _dataProvider.GetTeam(id1);
            var team2 = _dataProvider.GetTeam(id2);

            var sumPts = team1.Pts + team2.Pts;
            var sumPlusMinus = team1.PlusMinus + team2.PlusMinus;
            var ratio = (100 - sumPlusMinus) / 100;
            var result = Math.Round(ratio * sumPts);
            string total = ratio < 1 ? "ТБ" : "ТМ";

            return string.Concat(total, " ", result.ToString());
        }
    }
}