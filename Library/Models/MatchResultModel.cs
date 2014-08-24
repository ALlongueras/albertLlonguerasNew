using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class MatchResultModel : IBasePorraModel
    {
        public bool FinalOfMonth { get; set; }

        public string LocalTeam { get; set; }

        public string LocalScore { get; set; }

        public string VisitorTeam { get; set; }

        public string VisitorScore { get; set; }

        public string CurrentMonth { get; set; }
    }
}
