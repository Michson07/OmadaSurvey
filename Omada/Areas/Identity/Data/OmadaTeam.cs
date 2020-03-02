using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Omada.Areas.Identity.Data
{
    public class OmadaTeam
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool OpinionsVisible { get; set; }
    }
}
