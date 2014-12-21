using System;
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

        public static List<IPublishedContent> GetPlayersNode(IPublishedContent node)
        {
            var paco1 = node.Descendants("Jugador").ToList();
            return paco1;
        }

        public static List<IPublishedContent> GetPorresNode(IPublishedContent node)
        {
            var list = new List<IPublishedContent>();
            var node1 = Utils.GetRootNode(node);
            var identifier = Utils.GetMatchNode(node).GetPropertyValue("previaIdentifier").ToString();
            var paco = GetPlayersNode(node1);
            foreach (var content in paco)
            {
                if (content.Descendants().Any(x=>x.GetPropertyValue("porraIdentifier").ToString()==identifier))
                {
                    list.Add(content.Descendants().First(x => x.GetPropertyValue("porraIdentifier").ToString() == identifier));
                }
            }
            return list;
        }

        public static IPublishedContent GetMatchNode(IPublishedContent node)
        {
            node = Utils.GetRootNode(node);
            return node.DescendantsOrSelf("Previa").FirstOrDefault(x => x.GetPropertyValue("isActive").ToString() == "True");
        }

        public static string GetCurrentMonthOfPrevia(IPublishedContent node)
        {
            node = Utils.GetMatchNode(node);
            var month = node.GetPropertyValue("matchDay").ToString();
            var currentMonth = DateTime.Parse(month).Month;
            return currentMonth.ToString();
        }
    }
}
