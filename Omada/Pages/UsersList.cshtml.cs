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
using Omada.Models;

namespace Omada.Pages
{
    [Authorize(Roles = "Admin,Team Leader")]
    public class UsersListModel : PageModel
    {
        private readonly UserManager<OmadaUser> userManager;

        public IEnumerable<OmadaUser> Users { get; set; }
        public List<OmadaTeam> Teams { get; set; }

        public UsersListModel(UserManager<OmadaUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet()
        {
            Users = userManager.Users;
            LeaderTeam team = new LeaderTeam();
            Teams = team.GetTeams(userManager.GetUserId(HttpContext.User));
        }
    }
}