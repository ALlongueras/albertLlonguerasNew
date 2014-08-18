using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;
using Umbraco.Core.Models;

namespace Library.Businness
{
    public interface IPorraManager
    {
        List<PlayersInformation> GetWholePuntuationOfPlayers(IEnumerable<IPublishedContent> nodes, string identifier, MatchResultModel result);

        List<PlayersInformation> GetInformationOfPlayers(IEnumerable<IPublishedContent> nodes);

        List<PlayersInformation> AssignDRSToPlayers(List<PlayersInformation> informationList);

        List<PlayersInformation> UpdateResults(Dictionary<string, BasePorraModel> porras,
            List<PlayersInformation> informationList, MatchResultModel result);

        Dictionary<string, BasePorraModel> GetNewResult(IEnumerable<IPublishedContent> playerNodes, string identifier);

        void ApplyDRSToPlayers(IEnumerable<PlayersInformation> informationList);

        void FinalOfMonth(IEnumerable<PlayersInformation> informationList, MatchResultModel matchResult);

        void UpdateNewScore(IEnumerable<PlayersInformation> informationList);

        void AssignPuntuation(IEnumerable<PlayersInformation> informationList);
    }
}
