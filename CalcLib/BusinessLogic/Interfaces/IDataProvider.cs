using System.Collections.Generic;
using System.Threading.Tasks;
using CalcLib.Data;

namespace CalcLib.BusinessLogic.Interfaces
{
    public interface IDataProvider
    {
        List<Team> GetAllTeams();
        Team GetTeam(long id);
    }
}