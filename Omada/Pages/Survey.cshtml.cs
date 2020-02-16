﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;
using Omada.ManageTeamsAndSurveys;

namespace Omada.Pages.Shared
{
    public class SurveyModel : PageModel
    {
        private readonly SurveyData surveyData;
        private readonly UserManager<OmadaUser> userManager;
        public SurveyModel(SurveyData surveyData, UserManager<OmadaUser> userManager)
        {
            this.surveyData = surveyData;
            this.userManager = userManager;
        }
        [BindProperty]
        public OmadaSurvey Survey{ get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }        
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            Survey.UserId = userManager.GetUserId(HttpContext.User);
            Survey.Date = DateTime.UtcNow;
            surveyData.AddSurvey(Survey);
            return RedirectToPage("./SurveyCompleted");
        }
    }
}