using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;
using Omada.ManageTeamsAndSurveys;
using Omada.Pages.ManageUser;

namespace Omada.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddUserToTeamAsAdminModel : PageModel
    {
        private readonly ITeamData teamData;
        private readonly UserData userData;

        public OmadaUser AddedUser { get; set; }
        public List<OmadaTeam> Teams { get; set; }
        [BindProperty, Required]
        public int TeamId { get; set; }
        [BindProperty, Required]
        public int IsLeader { get; set; }

        public AddUserToTeamAsAdminModel(ITeamData teamData, UserData userData)
        {
            this.teamData = teamData;
            this.userData = userData;
        }
        public IActionResult OnGet(string userId)
        {
            AddedUser = userData.GetUserById(userId);
            Teams = teamData.GetAllTeams();
            if(AddedUser == null)
            {
                //return RedirectToPage("./NotFound");
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public IActionResult OnPost(string userId)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            AddUserToTeam addUserToTeam = new AddUserToTeam();
            addUserToTeam.AddUser(userId, TeamId, IsLeader);
            return RedirectToPage("./UsersList");
        }
    }
}