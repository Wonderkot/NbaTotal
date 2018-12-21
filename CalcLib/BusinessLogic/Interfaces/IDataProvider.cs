using System;
using System.Collections.Generic;
using CalcLib.Data;

namespace CalcLib.BusinessLogic.Interfaces
{
    public interface IDataProvider
    {
        Dictionary<long, string> GetAllTeams();
        Team GetTeam(long id);
        event Action<string> ShowMessage;
    }
}