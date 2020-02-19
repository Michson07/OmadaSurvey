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
    [Authorize(Roles="Admin")]
    public class DeleteTeamModel : PageModel
    {
        private readonly ITeamData teamData;
        [BindProperty]
        public OmadaTeam Team { get; set; }

        public DeleteTeamModel(ITeamData teamData)
        {
            this.teamData = teamData;
        }
        public IActionResult OnGet(int teamId)
        {
            Team = teamData.GetTeamById(teamId);
            if(Team == null)
            {
                //return RedirectToPage("./NotFound");
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public IActionResult OnPost(int teamId)
        {
            teamData.Delete(teamId);
            return RedirectToPage("./TeamsList");
        }
    }
}