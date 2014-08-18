using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Test.Businness
{
    public class PorraManagerInitializers
    {
        public static MatchResultModel InitializeMatchResult(MatchResultModel matchResult)
        {
            matchResult.FinalOfMonth = true;
            return matchResult;
        }

        public static Dictionary<string, BasePorraModel> InitializePorras(Dictionary<string, BasePorraModel> porras)
        {
            porras.Add("Alex", new BasePorraModel
            {
                LocalScore = "5,3",
                VisitorScore = "12"
            });
            porras.Add("Albert", new BasePorraModel
            {
                LocalScore = "20,5,4",
                VisitorScore = "12"
            });
            return porras;
        }

        public static List<PlayersInformation> InitialiazePlayersInformation(List<PlayersInformation> information)
        {
            information.Add(new PlayersInformation
            {
                PlayerName = "Alex",
                Information = new GlobalPlayerInformation
                {
                    OldInformation = new PlayerInformation
                    {
                        GlobalPuntuation = 10,
                        MonthPuntuation = (decimal)6.5,
                        PorreroPuntuation = 1
                    },
                    NewInformation = new PlayerInformation()
                }
            });
            information.Add(new PlayersInformation
            {
                PlayerName = "Albert",
                Information = new GlobalPlayerInformation
                {
                    OldInformation = new PlayerInformation
                    {
                        GlobalPuntuation = 8,
                        MonthPuntuation = (decimal)6.5,
                        PorreroPuntuation = 0
                    },
                    NewInformation = new PlayerInformation()
                }
            });
            information.Add(new PlayersInformation
            {
                PlayerName = "Xevi",
                Information = new GlobalPlayerInformation
                {
                    OldInformation = new PlayerInformation
                    {
                        GlobalPuntuation = 7,
                        MonthPuntuation = (decimal)6.3,
                        PorreroPuntuation = 4
                    },
                    NewInformation = new PlayerInformation()
                }
            });
            return information;
        }

    }
}
