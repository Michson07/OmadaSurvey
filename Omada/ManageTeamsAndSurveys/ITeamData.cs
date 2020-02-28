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
        List<OmadaUser> GetTeamUsers(int teamId);
        OmadaTeam Update(OmadaTeam team);
        List<NotTeamMember> UsersNotInTeam(int teamId);
        void AddUserToTeam(string userId, int teamId, int isLeader);
        List<OmadaTeam> GetLeaderTeams(string leaderId);
        List<OmadaTeam> GetUserTeams(string userId);
        List<OmadaUser> GetTeamLeaders(OmadaTeam team);
        List<OmadaTeam> GetTeamsWhereUserNotMember(string userId);
        void RemoveTeamMember(string userId, OmadaTeam team);
        void SetNoLeaders(OmadaTeam team);
        void UpdateLeaderStatus(string userId, OmadaTeam team);
    }

}
