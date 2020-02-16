using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;

namespace Omada.Pages.ManageUser
{
    public class EditUserModel : PageModel
    {
        private readonly IUserData userData;
        [BindProperty]
        public OmadaUser UpdatedUser { get; set; }

        public EditUserModel(IUserData userData)
        {
            this.userData = userData;
        }

        public IActionResult OnGet(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                UpdatedUser = userData.GetUserById(userId);
            }
            else
            {
                UpdatedUser = new OmadaUser();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if(String.IsNullOrEmpty(UpdatedUser.Id))
            {
                UpdatedUser = userData.Edit(UpdatedUser);
            }
            else
            {
                UpdatedUser = userData.Add(UpdatedUser);
            }
            userData.Commit();
            return RedirectToPage("../UsersList");
        }
    }
}