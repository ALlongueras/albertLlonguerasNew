using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Helpers
{
    public class PorraManagerUtils
    {
        public static string ConvertObjectToJson(IEnumerable<PlayersInformation> informationList)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(informationList);
            return json;
        }
    }
}
