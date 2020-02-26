using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Authorize(Roles = "Admin, Team Leader")]
    public class AddUserToTeamModel : PageModel
    {
        private readonly ITeamData teamData;
        private readonly UserData userData;
        private readonly UserManager<OmadaUser> userManager;

        public OmadaUser AddedUser { get; set; }
        public IEnumerable<OmadaTeam> Teams { get; set; }
        [BindProperty, Required]
        public int TeamId { get; set; }
        [BindProperty, Required]
        public int IsLeader { get; set; }

        public AddUserToTeamModel(ITeamData teamData, UserData userData, UserManager<OmadaUser> userManager)
        {
            this.teamData = teamData;
            this.userData = userData;
            this.userManager = userManager;
        }
        public IActionResult OnGet(string userId)
        {
            AddedUser = userData.GetUserById(userId);
            if(AddedUser == null)
            {
                //return RedirectToPage("./NotFound");
                return RedirectToPage("./Index");
            }
            if(User.IsInRole("Admin"))
            {
                Teams = teamData.GetTeamsWhereUserNotMember(AddedUser.Id);
            }
            else if(User.IsInRole("Team Leader"))
            {
                IEnumerable<OmadaTeam> userNotMemberInTeams = teamData.GetTeamsWhereUserNotMember(AddedUser.Id);
                IEnumerable<OmadaTeam> LeaderTeams = teamData.GetLeaderTeams(userManager.GetUserId(HttpContext.User));
                Teams = LeaderTeams.Where(u => userNotMemberInTeams.Any(l => l.Id == u.Id));
            }
            
            return Page();
        }

        public IActionResult OnPost(string userId)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            teamData.AddUserToTeam(userId, TeamId, IsLeader);
            return RedirectToPage("./UsersList");
        }
    }
}