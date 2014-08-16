using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BasePorraModel : IBasePorraModel
    {
        public string LocalTeam { get; set; }

        public string LocalScore { get; set; }

        public string VisitorTeam { get; set; }

        public string VisitorScore { get; set; }
    }

    public interface IBasePorraModel
    {
        string LocalTeam { get; set; }

        string LocalScore { get; set; }

        string VisitorTeam { get; set; }

        string VisitorScore { get; set; }
    }
}
