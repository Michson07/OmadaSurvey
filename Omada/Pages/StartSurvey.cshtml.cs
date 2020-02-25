using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;
using Omada.ManageTeamsAndSurveys;

namespace Omada.Pages.Shared
{
    [Authorize]
    public class StartSurveyModel : PageModel
    {
        private readonly SurveyData surveyData;
        private readonly UserManager<OmadaUser> userManager;
        public int WeekNumber { get; set; }
        public bool IsUserAbleToStart { get; set; }

        public StartSurveyModel(SurveyData surveyData, UserManager<OmadaUser> userManager)
        {
            this.surveyData = surveyData;
            this.userManager = userManager;
        }
        public void OnGet()
        {
            CultureInfo culture = new CultureInfo("en-US");
            Calendar calendar = culture.Calendar;
            CalendarWeekRule calendarWeekRule = culture.DateTimeFormat.CalendarWeekRule;
            DayOfWeek dayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
            WeekNumber = calendar.GetWeekOfYear(DateTime.UtcNow, calendarWeekRule, dayOfWeek);
            IsUserAbleToStart = surveyData.CheckIfUserHaveDoneSurveyThisWeek(userManager.GetUserId(HttpContext.User));
        }
    }
}