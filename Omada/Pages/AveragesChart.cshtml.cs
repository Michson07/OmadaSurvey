using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Omada.Areas.Identity.Data;
using Omada.Average;
using Omada.ManageTeamsAndSurveys;

namespace Omada.Pages
{
    [Authorize(Roles = "Admin, Team Leader")]
    public class AveragesChartModel : PageModel
    {
        private readonly AveragesCalculate averagesCalculate;
        private readonly UserManager<OmadaUser> userManager;

        public List<OmadaSurveysAverage> AverageWeeks { get; set; }
        public AveragesChartModel(AveragesCalculate averagesCalculate, UserManager<OmadaUser> userManager)
        {
            this.averagesCalculate = averagesCalculate;
            this.userManager = userManager;
        }
        public void OnGet()
        {
            LeaderTeam team = new LeaderTeam();
            string leaderId = userManager.GetUserId(HttpContext.User);
            AverageWeeks = averagesCalculate.GetSurveysAverages(team.GetTeam(leaderId));
        }
    }
}