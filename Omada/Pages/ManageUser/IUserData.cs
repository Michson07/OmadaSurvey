using Omada.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.Pages.ManageUser
{
    public interface IUserData
    {
        OmadaUser Edit(OmadaUser updatedUser);
        OmadaUser Add(OmadaUser newUser);
        OmadaUser Delete(string id);
        OmadaUser GetUserById(string id);
        public string GetUserEmail(OmadaUser user);
        int Commit();
        OmadaUser GetUserByName(string nick);

    }
}
