using Omada.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.ManageTeamsAndSurveys
{
    public interface ITeamData
    {
        List<OmadaTeam> GetAllTeams();
        OmadaTeam Add(OmadaTeam team);
        void Delete(int teamId);
        OmadaTeam GetTeamById(int teamId);
    }
}
