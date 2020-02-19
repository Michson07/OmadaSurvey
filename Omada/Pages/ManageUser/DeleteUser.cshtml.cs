using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Omada.Areas.Identity.Data;

namespace Omada.Pages.ManageUser
{
    public class DeleteUserModel : PageModel
    {
        private readonly IUserData userData;
        public OmadaUser DeletedUser { get; set; }

        public DeleteUserModel(IUserData userData)
        {
            this.userData = userData;
        }
        public IActionResult OnGet(string userId)
        {
            DeletedUser = userData.GetUserById(userId);
            if(DeletedUser == null)
            {
                //return RedirectToPage("./NotFound");
                return RedirectToPage("./Index");
            }
            return Page();
        }
        public IActionResult OnPost(string userId)
        {
            DeletedUser = userData.Delete(userId);
            userData.Commit();
            if(DeletedUser == null)
            {
                //return RedirectToPage("./NotFound");
                return RedirectToPage("./Index");
            }
            TempData["Message"] = $"{DeletedUser.Email} deleted";
            return RedirectToPage("../UsersList");
        }
    }
}