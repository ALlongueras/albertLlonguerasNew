using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace Library.Businness
{
    public interface IPorraManager
    {
        List<PlayersInformation> GetWholePuntuationOfPlayers(List<IPublishedContent> nodes, string identifier, MatchResultModel result);

        List<PlayersInformation> GetInformationOfPlayers(List<IPublishedContent> nodes, string currentMonth);

        List<PlayersInformation> AssignDRSToPlayers(List<PlayersInformation> informationList);

        List<PlayersInformation> UpdateResults(Dictionary<string, BasePorraModel> porras,
            List<PlayersInformation> informationList, MatchResultModel result);

        Dictionary<string, BasePorraModel> GetNewResult(IEnumerable<IPublishedContent> playerNodes, string identifier);

        void ApplyDRSToPlayers(IEnumerable<PlayersInformation> informationList);

        void FinalOfMonth(IEnumerable<PlayersInformation> informationList, MatchResultModel matchResult);

        void UpdateNewScore(IEnumerable<PlayersInformation> informationList);

        void AssignPuntuation(IEnumerable<PlayersInformation> informationList);

        bool IsValidPorraAcordingTime(IPublishedContent porraNode, object matchDay);

        Dictionary<string, BasePorraModel> GetPorrasFromPlayers(IPublishedContent node);
    }
}
