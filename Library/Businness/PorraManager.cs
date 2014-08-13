using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
//using Umbraco.Core.Models;
//using umbraco.MacroEngines;
//using Umbraco.Web;
using Umbraco.Web;
using Umbraco.Core.Models;

namespace Library.Businness
{
    public class PorraManager
    {
        public static string GetMatchIdentifier(IPublishedContent node)
        {
            var previaNode = GetMatchNode(node);
            return previaNode.GetPropertyValue("previaIdentifier").ToString();
        }

        public static IPublishedContent GetMatchNode(IPublishedContent node)
        {
            return node.DescendantsOrSelf("Previa").FirstOrDefault(x => x.GetPropertyValue("isActive").ToString() == "True");
        }
    }
}
