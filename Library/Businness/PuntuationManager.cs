using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

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
    }
}
