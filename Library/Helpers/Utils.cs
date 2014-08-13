using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using umbraco.presentation.translation;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Library.Helpers
{
    public class Utils
    {
        public static IPublishedContent GetPlayerNode(RenderModel node, string player)
        {
            var paco = node.Content.DescendantsOrSelf("Jugador");
            var paco2 = paco.FirstOrDefault(x => x.GetPropertyValue("name").ToString() == player);
            return paco2;
        }

        public static bool HasPorraAccordingIdentifier(IPublishedContent node, string identifier)
        {
            return node.DescendantsOrSelf("Porra").Any(x => x.GetPropertyValue("porraIdentifier").ToString() == identifier);
        }
    }
}
