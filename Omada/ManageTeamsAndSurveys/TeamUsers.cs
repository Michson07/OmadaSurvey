using Omada.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.ManageTeamsAndSurveys
{
    public class TeamUsers
    {
        public OmadaUser User { get; set; }
        public bool IsSelected { get; set; }
    }
}
