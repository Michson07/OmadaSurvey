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
using System.Net.Mail;
using System.Net;
using Omada.Pages.ManageUser;

namespace Omada.Pages
{
    [Authorize]
    [BindProperties]
    public class TeamsListModel : PageModel
    {
        public ITeamData teamData;
        private readonly UserManager<OmadaUser> userManager;
        private readonly IUserData userData;
        public OmadaTeam Team { get; set; }
        public bool IsUserAbleToEdit { get; set; }
        public Dictionary<OmadaTeam, List<OmadaUser>> Teams_Users = new Dictionary<OmadaTeam, List<OmadaUser>>();

        public TeamsListModel(ITeamData teamData, UserManager<OmadaUser> userManager, IUserData userData)
        {
            this.teamData = teamData;
            this.userManager = userManager;
            this.userData = userData;
        }
        public bool CheckIfUserInTeam(int teamId)
        {
            return teamData.GetUserTeams(userManager.GetUserId(HttpContext.User)).Where(t => t.Id == teamId).Any();
        }
        public bool CheckIfUserIsLeader(int teamId)
        {
            return teamData.GetLeaderTeams(userManager.GetUserId(HttpContext.User)).Where(t => t.Id == teamId).Any();
        }
        public void OnGet()
        {
            IEnumerable<OmadaTeam> teams = new List<OmadaTeam>();
            if (User.IsInRole("Admin"))
            {
                teams = teamData.GetAllTeams();
            }
            else
            {
                teams = teamData.GetAllTeams().Where(t => t.IsPublic == true).ToList();
                IEnumerable<OmadaTeam> privateUserTeams = (teamData.GetUserTeams(userManager.GetUserId(User)).Where(t => t.IsPublic == false)).ToList();
                teams = teams.Concat(privateUserTeams);
            }
            foreach (var team in teams)
            {
                List<OmadaUser> teamUsers = new List<OmadaUser>();
                teamUsers = teamData.GetTeamUsers(team.Id);
                Teams_Users.Add(team, teamUsers);
            }
        }
        public ActionResult OnPost([FromBody]int teamId)
        {
            Team = teamData.GetTeamById(teamId);
            List<OmadaUser> leaders = teamData.GetTeamLeaders(Team);
            EmailSender emailSender = new EmailSender();
            foreach (var leader in leaders)
            {
                string email = leader.Email;
                string userId = userManager.GetUserId(HttpContext.User);
                OmadaUser user = userManager.FindByIdAsync(userId).Result;
                string subject = "Request to be added to team ";
                string body = $"{user.UserName} wants to join your team {Team.Name}";
                if(!emailSender.SendEmail(email, subject, body))
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred during sending email! Please, try again later");
                }
            }
            return StatusCode((int)HttpStatusCode.OK, "Request Sent");
        }
    }
}