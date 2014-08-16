using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace AlbertLlonguerasNew.Models
{
    public class NewResultModel : NewPorraModel
    {
        public bool FinalOfMonth { get; set; }

        public IEnumerable<IPublishedContent> PlayerNodes { get; set; } 
    }
}