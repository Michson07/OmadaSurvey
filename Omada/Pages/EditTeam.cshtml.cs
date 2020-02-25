using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;
using Omada.ManageTeamsAndSurveys;

namespace Omada.Pages
{
    [BindProperties]
    [Authorize]
    public class EditTeamModel : PageModel
    {
        private readonly ITeamData teamData;
        private readonly UserManager<OmadaUser> userManager;
        public OmadaTeam Team { get; set; }
        public List<TeamUsers> TeamUsersList { get; set; }
        public EditTeamModel(ITeamData teamData, UserManager<OmadaUser> userManager)
        {
            this.teamData = teamData;
            this.userManager = userManager;
        }
        public IActionResult OnGet(int? teamId)
        {
            if(!teamId.HasValue)
            {
                Team = new OmadaTeam();
                TeamUsersList = new List<TeamUsers>();
                foreach(var user in userManager.Users)
                {
                    TeamUsersList.Add(new TeamUsers()
                    {
                        User = user,
                        IsSelected = false
                    });
                }
            }
            else
            {
                Team = teamData.GetTeamById(teamId.Value);
                TeamUsersList = teamData.UsersNotInTeam(teamId.Value);
            }
            if(Team == null)
            {
                return RedirectToPage("./Index");
                //return RedirectToPage("./NotFound");
            }
            
            return Page();
        }
        public IActionResult OnPost ()
        {
            if (!ModelState.IsValid)
            {
                return Page();  
            }

            OmadaTeam team = new OmadaTeam();
            if (Team.Id > 0)
            {
                team = teamData.Update(Team);
            }
            else
            {
                team = teamData.Add(Team);
            }
            var selectedUsers = TeamUsersList.Where(t => t.IsSelected == true).Select(t => t.User.Id).ToList();
            foreach (var user in selectedUsers)
            {
                teamData.AddUserToTeam(user, team.Id);
            }

            return RedirectToPage("./TeamsList");
        }
    }
}