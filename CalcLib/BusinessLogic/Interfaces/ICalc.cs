using System;

namespace CalcLib.BusinessLogic.Interfaces
{
    public interface ICalc
    {
        dynamic GetResultByTeam(long id1, long id2);
        event Action<string> ShowMessage;
        void SetShowMessage();
    }
}