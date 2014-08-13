using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using DropDownList = umbraco.editorControls.SettingControls.DropDownList;

namespace AlbertLlonguerasNew.Models
{
    public class NewPorraModel
    {
        public string Player { get; set; }

        public IPublishedContent PorraNode { get; set; }

        public string MatchIdentifier { get; set; }

        public string LocalTeam { get; set; }

        public string LocalScore { get; set; }

        public string VisitorTeam { get; set; }
        
        public string VisitorScore { get; set; }
    }
}