using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using umbraco;
using Umbraco.Core.Models;
using umbraco.NodeFactory;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

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

        public static IPublishedContent GetRootNode(IPublishedContent node)
        {
            return
                node.AncestorsOrSelf("Page")
                    .FirstOrDefault(x => x.GetPropertyValue("identifier").ToString() == "homePorra");
        }

        public static IEnumerable<IPublishedContent> GetPlayersNode(IPublishedContent node)
        {
            //var rootNode = 
            //var paco = rootNode.
            ////var pa = GetRootNode(node);
            //var paco =
            //    node.GetAncestorNodes().Where(node1=>node1.NodeTypeAlias=="Page")
            //        .FirstOrDefault(x => x.GetProperty("identifier").Value.ToString() == "homePorra");
            var paco1 = node.Descendants("Jugador");
            return paco1;
        }
    }
}
