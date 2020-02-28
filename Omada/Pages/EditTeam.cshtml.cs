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
        public List<NotTeamMember> NotTeamMembers { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
        public bool TeamExists { get; set; }
        public EditTeamModel(ITeamData teamData, UserManager<OmadaUser> userManager)
        {
            this.teamData = teamData;
            this.userManager = userManager;
        }

        private void setUsers(int? teamId)
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

        public IActionResult OnGet(int? teamId)
        {
            setUsers(teamId);
            if (Team == null)
            {
                return RedirectToPage("./Index");
                //return RedirectToPage("./NotFound");
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

        private void RemoveTeamMembers(OmadaTeam team)
        {
            List<OmadaUser> removedMembers = TeamMembers.Where(m => m.Remove == true).Select(m => m.User).ToList();
            foreach(var member in removedMembers)
            {
                teamData.RemoveTeamMember(member.Id, team);
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
        public IActionResult OnPost ()
        {
            if (!ModelState.IsValid)
            {
                setUsers(Team.Id);
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
                teamData.AddUserToTeam(userManager.GetUserId(User), team.Id, 1);
            }

            AddNewUsersToTeam(team);
            RemoveTeamMembers(team);
            UpdateLeaders(team);

            return RedirectToPage("./TeamsList");
        }
    }
}