using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.Models;
//using umbraco.editorControls.SettingControls;

namespace Library.Businness
{
    public class TeamsManager
    {
        private static Dictionary<string,List<string>> GetTeams()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "json\\teams.json");
            using (var reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                var jsonTeams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,List<string>>>(json);
                return jsonTeams;
            }
        }

        public static List<TeamModel> LoadTeams()
        {
            var teams = GetTeams();
            var teamsModified = new List<TeamModel>();
            foreach (var regionTeam in teams)
            {
                teamsModified.AddRange(regionTeam.Value.Select(team => new TeamModel
                {
                    Region = regionTeam.Key, Team = team
                }));
            }
            return teamsModified;
        }
    }
}
