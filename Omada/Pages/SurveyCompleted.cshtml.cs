using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;
using Omada.Average;
using Omada.ManageTeamsAndSurveys;

namespace Omada.Pages
{
    public class SurveyCompletedModel : PageModel
    {
        private readonly SurveyData surveyData;
        private readonly UserManager<OmadaUser> userManager;
        private readonly ITeamData teamData;
        private readonly AveragesCalculate averagesCalculate;

        public List<OmadaTeam> Teams { get; set; }
        public Dictionary<string, List<OmadaSurveysAverage>> AverageWeeks = new Dictionary<string, List<OmadaSurveysAverage>>();
        public bool IsSurveyNotCompleted { get; set; }
        public SurveyCompletedModel(SurveyData surveyData, UserManager<OmadaUser> userManager, ITeamData teamData, AveragesCalculate averagesCalculate)
        {
            this.surveyData = surveyData;
            this.userManager = userManager;
            this.teamData = teamData;
            this.averagesCalculate = averagesCalculate;
        }
        public IActionResult OnGet()
        {
            string userId = userManager.GetUserId(HttpContext.User);
            IsSurveyNotCompleted = surveyData.CheckIfUserHaveDoneSurveyThisWeek(userId);
            if (IsSurveyNotCompleted)
            {
                return RedirectToPage("./Survey");
            }
            Teams = teamData.GetUserTeams(userId);
            foreach(var team in Teams)
            {
                var averages = averagesCalculate.GetSurveysAverages(team);
                AverageWeeks.Add(team.Name, averages);
            }
            return Page();
        }
    }
}