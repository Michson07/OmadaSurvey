using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;
using Omada.ManageTeamsAndSurveys;

namespace Omada.Pages
{
    [Authorize(Roles = "Admin")]
    public class TeamsListModel : PageModel
    {
        private readonly ITeamData teamData;
        public Dictionary<OmadaTeam, List<OmadaUser>> Teams_Users = new Dictionary<OmadaTeam, List<OmadaUser>>();

        public TeamsListModel(ITeamData teamData)
        {
            this.teamData = teamData;
        }
        public void OnGet()
        {
            List<OmadaTeam> teams = teamData.GetAllTeams();
            foreach (var team in teams)
            {
                List<OmadaUser> teamUsers = new List<OmadaUser>();
                teamUsers = teamData.GetTeamUsers(team.Id);
                Teams_Users.Add(team, teamUsers);
            }
        }
    }
}