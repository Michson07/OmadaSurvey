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
using System.Text.Json;
using System.Text.Json.Serialization;
using Omada.Pages.ManageUser;

namespace Omada.Pages
{
    [BindProperties(SupportsGet = true)]
    [Authorize]
    public class EditTeamModel : PageModel
    {
        private readonly ITeamData teamData;
        private readonly UserManager<OmadaUser> userManager;
        private readonly IUserData userData;

        public OmadaTeam Team { get; set; }
        public List<NotTeamMember> NotTeamMembers { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
        public bool TeamExists { get; set; }
        public string membersJson;
        public EditTeamModel(ITeamData teamData, UserManager<OmadaUser> userManager, IUserData userData)
        {
            this.teamData = teamData;
            this.userManager = userManager;
            this.userData = userData;
        }

        private void SetUsers(int? teamId)
        {
            if (!teamId.HasValue)
            {
                TeamExists = false;
                Team = new OmadaTeam();
                NotTeamMembers = new List<NotTeamMember>();
                foreach (var user in userManager.Users.Where(u => u.Id != userManager.GetUserId(User)))
                {
                    NotTeamMembers.Add(new NotTeamMember()
                    {
                        User = user,
                        IsSelected = false
                    });
                }
            }
            else
            {
                TeamExists = true;
                Team = teamData.GetTeamById(teamId.Value);
                NotTeamMembers = teamData.UsersNotInTeam(Team.Id);
                TeamMembers = new List<TeamMember>();
                foreach(var member in teamData.GetTeamUsers(Team.Id))
                {
                    TeamMembers.Add(new TeamMember()
                    {
                        User = member,
                        IsLeader = teamData.GetTeamLeaders(Team).Where(l => l.Id == member.Id).Any() ? true : false,
                        Remove = false
                    });
                    
                }
            }
        }

        private void SerializeMembers()
        {
            membersJson = JsonSerializer.Serialize(NotTeamMembers.Select(m => m.User.UserName).ToList());
        }
        public IActionResult OnGet(int? teamId)
        {
            SetUsers(teamId);
            SerializeMembers();
            if (Team == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }

        private void AddNewUsersToTeam(OmadaTeam team)
        {
            var selectedUsers = NotTeamMembers.Where(u => u.IsSelected == true).Select(u => u.User.Id).ToList();
            foreach (var user in selectedUsers)
            {
                teamData.AddUserToTeam(user, team.Id, 0);
            }
        }

        private void UpdateLeaders(OmadaTeam team)
        {
            List<OmadaUser> updatedLeaders = TeamMembers.Where(m => m.IsLeader == true).Select(m => m.User).ToList();
            teamData.SetNoLeaders(team);
            foreach(var member in updatedLeaders)
            {
                teamData.UpdateLeaderStatus(member.Id, team);
            }
        }

        public void OnPost([FromBody] UserTeamJSON userTeamJSON)
        {
            var user = userData.GetUserByName(userTeamJSON.Nick);
            int team = 0;
            try
            {
                team = Int32.Parse(userTeamJSON.Team);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            teamData.RemoveTeamMember(user.Id, team);
        }

        public async Task<IActionResult> OnPostFinalAsync ()
        {
            if (!ModelState.IsValid)
            {
                SetUsers(Team.Id);
                return Page();  
            }

            if (Team.Id > 0)
            {
                OmadaTeam team = teamData.Update(Team);
                AddNewUsersToTeam(team);
                UpdateLeaders(team);
            }
            else
            {
                OmadaTeam team = teamData.Add(Team, userManager.GetUserId(User));
            }

            await HttpContext.RefreshLoginAsync();
            return RedirectToPage("./TeamsList");
        }
    }
}