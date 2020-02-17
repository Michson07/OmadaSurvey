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
    public class TeamsModel : PageModel
    {
        private readonly ITeamData teamData;
        public List<OmadaTeam> Teams { get; set; }

        public TeamsModel(ITeamData teamData)
        {
            this.teamData = teamData;
        }
        public void OnGet()
        {
            Teams = teamData.GetAllTeams();
        }
    }
}