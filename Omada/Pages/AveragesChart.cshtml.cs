﻿using System;
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
        private readonly TeamData teamData;

        public List<OmadaTeam> Teams { get; set; }
        public Dictionary<string, List<OmadaSurveysAverage>> AverageWeeks = new Dictionary<string, List<OmadaSurveysAverage>>();
        public AveragesChartModel(AveragesCalculate averagesCalculate, UserManager<OmadaUser> userManager, TeamData teamData)
        {
            this.averagesCalculate = averagesCalculate;
            this.userManager = userManager;
            this.teamData = teamData;
        }
        public void OnGet()
        {
            string leaderId = userManager.GetUserId(HttpContext.User);
            Teams = teamData.GetLeaderTeams(leaderId);
            foreach(var team in Teams)
            {
                var averages = averagesCalculate.GetSurveysAverages(team);
                AverageWeeks.Add(team.Name, averages);
            }
        }
    }
}