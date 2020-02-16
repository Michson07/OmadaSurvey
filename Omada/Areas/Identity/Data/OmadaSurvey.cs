using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.Areas.Identity.Data
{
    public class OmadaSurvey
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FirstAnswer { get; set; }
        public string SecondAnswer { get; set; }
        public string ThirdAnswer { get; set; }
        public DateTime Date { get; set; }
    }
}
