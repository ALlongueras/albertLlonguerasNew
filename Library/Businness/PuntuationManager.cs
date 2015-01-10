using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Helpers;
using Library.Models;
using Umbraco.Core.Models;
using umbraco.presentation.webservices;

namespace Library.Businness
{
    public class PuntuationManager
    {
        public Dictionary<string, string> GetPuntuationOfCurrentMonth(List<IPublishedContent> players)
        {
            var currentMonth = DateTime.Now.Month;
            var information = players.ToDictionary(player =>
                player.GetProperty("name").Value.ToString(),
                player => player.GetProperty(string.Format("month{0}", currentMonth)).Value.ToString());
            return information;
        }

        public List<PlayerPuntuation> GetPuntuationByMonthNoPorrero(IPublishedContent node)
        {
            var list = new List<PlayerPuntuation>();
            const int firstMonth = 8;
            var lastMonth = DateTime.Now.Month;
            var nodesPlayers = Utils.GetPlayersNode(node);
            //var values = new string[lastMonth - firstMonth + 1];
            foreach (var player in nodesPlayers)
            {
                var values = new List<decimal>();
                for (int i = 0; i <= lastMonth - firstMonth; i++)
                {
                    values.Add(Decimal.Parse(player.GetProperty("month" + (i + firstMonth)).Value.ToString()));
                }
                list.Add(new PlayerPuntuation
                {
                    Name = player.GetProperty("name").Value.ToString(),
                    Puntuation = values
                });
            }

            return list;
        }

        public List<PlayerPuntuation> GetPuntuationByMonth(IPublishedContent node)
        {
            var list = new List<PlayerPuntuation>();
            const int firstMonth = 0;
            var lastMonth = 0;
            var nodesPlayers = Utils.GetPlayersNode(node);
            //var values = new string[lastMonth - firstMonth + 1];
            foreach (var player in nodesPlayers)
            {
                var values = new List<decimal>();
                for (int i = 1; i <= 12; i++)
                {
                    values.Add(Decimal.Parse(player.GetProperty("month" + (i + firstMonth)).Value.ToString()) + Decimal.Parse(player.GetProperty("porreroMonth" + (i + firstMonth)).Value.ToString()) + Decimal.Parse(player.GetProperty("drsMonth" + (i + firstMonth)).Value.ToString()));
                }
                list.Add(new PlayerPuntuation
                {
                    Name = player.GetProperty("name").Value.ToString(),
                    Puntuation = values
                });
            }

            return list;
        }
    }
}
