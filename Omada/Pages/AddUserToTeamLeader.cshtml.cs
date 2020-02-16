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
using Omada.Pages.ManageUser;

namespace Omada.Pages
{
    [Authorize(Roles = "Team Leader")]
    public class AddUserToTeamLeaderModel : PageModel
    {
        private readonly ITeamData teamData;
        private readonly IUserData userData;

        public OmadaUser AddedUser { get; set; }
        public OmadaTeam AddedTeam { get; set; }
        public AddUserToTeamLeaderModel(TeamData teamData, UserData userData)
        {
            this.teamData = teamData;
            this.userData = userData;
        }
        public IActionResult OnGet(string userId, int teamId)
        {
            AddedUser = userData.GetUserById(userId);
            AddedTeam = teamData.GetTeamById(teamId);
            if (AddedUser == null || AddedTeam == null)
            {
                //return RedirectToPage("./NotFound");
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public IActionResult OnPost(string userId, int teamId)
        {
            AddUserToTeam addUserToTeam = new AddUserToTeam();
            addUserToTeam.AddUser(userId, teamId, 0);
            return RedirectToPage("./UsersList");
        }
    }
}