using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.UI;
using Library.Helpers;
using umbraco;
using umbraco.BusinessLogic;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Core.Models;
using Library.Models;
using umbraco.cms.businesslogic.web;
using umbraco.businesslogic;
using Umbraco.Web.Models;
using Umbraco.Web.UI.Umbraco.Dialogs;

namespace Library.Businness
{
    public class PorraManager : IPorraManager
    {

        public static string GetMatchIdentifier(IPublishedContent node)
        {
            node = Utils.GetRootNode(node);
            var previaNode = Utils.GetMatchNode(node);
            return previaNode.GetPropertyValue("previaIdentifier").ToString();
        }

        public List<PlayersInformation> GetWholePuntuationOfPlayers(List<IPublishedContent> nodes, string identifier, MatchResultModel matchResult)
        {
            var informationList = new List<PlayersInformation>();
            informationList = this.GetInformationOfPlayers(nodes, matchResult.CurrentMonth);
            if (matchResult.FinalOfMonth)
            {
                this.FinalOfMonth(informationList, matchResult);
                return informationList;
            }
            informationList = !matchResult.NotDRS ? this.AssignDRSToPlayers(informationList) : informationList;
            var porras = this.GetNewResult(nodes, identifier);
            informationList = this.UpdateResults(porras, informationList, matchResult);
            //this.ApplyDRSToPlayers(informationList);
            this.UpdateNewScore(informationList);
            this.AssignPuntuation(informationList);
            return informationList;
        }

        public List<PlayersInformation> GetInformationOfPlayers(List<IPublishedContent> nodes, string currentMonth)
        {
            var informationList = new List<PlayersInformation>();
            if (!nodes.Any()) return informationList;
            //var currentMonth = Utils.GetCurrentMonthOfPrevia(nodes.First());
            nodes.ForEach(x =>
                informationList.Add(new PlayersInformation
                {
                    PlayerName = x.GetPropertyValue("name").ToString(),
                    Information = new GlobalPlayerInformation
                    {
                        OldInformation = new PlayerInformation
                        {
                            Position = Convert.ToInt32(x.GetPropertyValue("position")),
                            GlobalPuntuation = Convert.ToDecimal(x.GetPropertyValue("globalPuntuation").ToString()),
                            MonthPuntuation = Convert.ToDecimal(x.GetPropertyValue(string.Format("month{0}", currentMonth)).ToString()),
                            LastScore = Convert.ToDecimal(x.GetPropertyValue("lastScore").ToString()),
                            PorreroPuntuation = Convert.ToDecimal(x.GetPropertyValue(string.Format("porreroMonth{0}", currentMonth)).ToString()),
                            DRSPuntuation = Convert.ToDecimal(x.GetPropertyValue(string.Format("drsMonth{0}", currentMonth)).ToString())
                        },
                        NewInformation = new PlayerInformation()
                    }
                }));
            return informationList;
        }

        public List<PlayersInformation> AssignDRSToPlayers(List<PlayersInformation> informationList)
        {
            var numberOfplayers = informationList.Count();
            var iterator = 1;
            informationList =
                informationList.OrderByDescending(x => x.Information.OldInformation.GlobalPuntuation).ToList();
            foreach (var player in informationList)
            {
                if (player == informationList.First()) continue;
                if (informationList.Any(x =>
                    x.Information.OldInformation.GlobalPuntuation - player.Information.OldInformation.GlobalPuntuation < 5
                    && x.Information.OldInformation.GlobalPuntuation - player.Information.OldInformation.GlobalPuntuation > 0))
                {
                    player.Information.NewInformation.HasDRS = true;
                }
            }
            return informationList;
        }

        public Dictionary<string, BasePorraModel> GetNewResult(IEnumerable<IPublishedContent> playerNodes, string identifier)
        {
            var porraList = new Dictionary<string, BasePorraModel>();
            foreach (var playerNode in playerNodes)
            {
                //Here it is necessary to do a validation regarding the porra date.
                if (playerNode.Descendants().Any(x => x.GetPropertyValue("porraIdentifier").ToString() == identifier))
                {
                    var porraNode =
                        playerNode.Descendants()
                            .FirstOrDefault(x => x.GetPropertyValue("porraIdentifier").ToString() == identifier);
                    porraList.Add(playerNode.GetPropertyValue("name").ToString(), new BasePorraModel
                    {
                        LocalTeam = porraNode.GetPropertyValue("localTeam").ToString(),
                        LocalScore = porraNode.GetPropertyValue("localScore").ToString(),
                        VisitorTeam = porraNode.GetPropertyValue("visitorTeam").ToString(),
                        VisitorScore = porraNode.GetPropertyValue("visitorScore").ToString()
                    });
                }
            }
            return porraList;
        }

        public List<PlayersInformation> UpdateResults(Dictionary<string, BasePorraModel> porras, List<PlayersInformation> informationList, MatchResultModel result)
        {
            foreach (var porra in porras)
            {
                var bonusMatchLocal = false;
                var bonusMatchVisitant = false;
                if (this.CheckIfTeamsAreCorrect(porra.Value, result))
                {
                    if (this.UpdatePorras(informationList, porra.Value.LocalScore, result.LocalScore, porra.Key))
                    {
                        CheckScorers(informationList, porra.Value.LocalScore, result.LocalScore, porra.Key);
                        bonusMatchLocal = true;
                    }
                    if (this.UpdatePorras(informationList, porra.Value.VisitorScore, result.VisitorScore, porra.Key))
                    {
                        CheckScorers(informationList, porra.Value.VisitorScore, result.VisitorScore, porra.Key);
                        bonusMatchVisitant = true;
                    }
                    if (bonusMatchLocal && bonusMatchVisitant)
                    {
                        this.ApplyMatchBonus(informationList, porra.Key);
                    }
                }
            }
            this.UpdateMonthPuntuation(informationList);
            return informationList;
        }

        private void UpdateMonthPuntuation(IEnumerable<PlayersInformation> informationList)
        {
            foreach (var player in informationList)
            {
                player.Information.NewInformation.MonthPuntuation = player.Information.OldInformation.MonthPuntuation +
                                                                    player.Information.NewInformation.LastScore;
            }
        }

        private bool CheckIfTeamsAreCorrect(BasePorraModel porra, MatchResultModel matchResult)
        {
            return porra.LocalTeam == matchResult.LocalTeam
                   && porra.VisitorTeam == matchResult.VisitorTeam;
        }

        private bool UpdatePorras(IEnumerable<PlayersInformation> informationList, string porraScores, string matchScores, string player)
        {
            var porraScoresCount = string.IsNullOrEmpty(porraScores) ? 0 : porraScores.Split(',').Count();
            var matchScoresCount = string.IsNullOrEmpty(matchScores) ? 0 : matchScores.Split(',').Count();
            if (porraScoresCount == matchScoresCount)
            {
                informationList.First(p => p.PlayerName == player).Information.NewInformation.LastScore++;
                return true;
            }
            return false;
        }

        private void CheckScorers(IEnumerable<PlayersInformation> informationList, string porraScores, string matchScores, string player)
        {
            var porraScoresList = new List<string>();
            var scores = string.IsNullOrEmpty(porraScores) ? null : Regex.Replace(porraScores, @"\s+", "").Split(',');
            if (scores != null && scores.Any())
            {
                porraScoresList.AddRange(scores);

            }
            scores = string.IsNullOrEmpty(matchScores) ? null : Regex.Replace(matchScores, @"\s+", "").Split(',');
            var matchScoresList = new List<string>();
            if (scores != null && scores.Any())
            {
                matchScoresList.AddRange(scores);

            }
            var puntuation = new double();
            foreach (var score in porraScoresList)
            {
                if (matchScoresList.Contains(score))
                {
                    puntuation += 0.5;
                    matchScoresList.Remove(score);
                }
            }
            informationList.First(p => p.PlayerName == player).Information.NewInformation.LastScore += (decimal)puntuation;
        }

        public void ApplyDRSToPlayers(IEnumerable<PlayersInformation> informationList)
        {
            foreach (var player in informationList)
            {
                player.Information.NewInformation.DRSPuntuation = player.Information.OldInformation.DRSPuntuation;
            }
            foreach (var player in informationList.Where(p => p.Information.NewInformation.HasDRS))
            {
                player.Information.NewInformation.DRSPuntuation += player.Information.NewInformation.LastScore * (decimal)0.5;
                player.Information.NewInformation.LastScore = player.Information.NewInformation.LastScore * (decimal)1.5;
            }
        }

        private void ApplyMatchBonus(IEnumerable<PlayersInformation> informationList, string player)
        {
            informationList.FirstOrDefault(p => p.PlayerName == player).Information.NewInformation.LastScore++;
        }

        public void FinalOfMonth(IEnumerable<PlayersInformation> informationList, MatchResultModel matchResult)
        {
            if (!matchResult.FinalOfMonth) return;
            var playersInformations = informationList as IList<PlayersInformation> ?? informationList.ToList();
            var max = playersInformations.OrderByDescending(x => x.Information.OldInformation.MonthPuntuation).FirstOrDefault().Information.OldInformation.MonthPuntuation;
            var porrerosOfMonth = new List<PlayersInformation>();
            porrerosOfMonth.AddRange(playersInformations.Where(x => (x.Information.OldInformation.MonthPuntuation == max)));
            var valueOfPorrero = (decimal)(5.0 / porrerosOfMonth.Count());
            foreach (var playerInformation in informationList)
            {
                playerInformation.Information.NewInformation.MonthPuntuation = playerInformation.Information.OldInformation.MonthPuntuation;
                playerInformation.Information.NewInformation.DRSPuntuation = playerInformation.Information.OldInformation.DRSPuntuation;
                if (porrerosOfMonth.Contains(playerInformation))
                {
                    //playerInformation.Information.NewInformation.LastScore += valueOfPorrero;
                    playerInformation.Information.NewInformation.PorreroPuntuation =
                        playerInformation.Information.OldInformation.PorreroPuntuation + valueOfPorrero;
                    playerInformation.Information.NewInformation.GlobalPuntuation =
                        playerInformation.Information.OldInformation.GlobalPuntuation + valueOfPorrero;
                }
                else
                {
                    playerInformation.Information.NewInformation.PorreroPuntuation = playerInformation.Information.OldInformation.PorreroPuntuation;
                    playerInformation.Information.NewInformation.GlobalPuntuation = playerInformation.Information.OldInformation.GlobalPuntuation;
                }
            }
        }

        public void UpdateNewScore(IEnumerable<PlayersInformation> informationList)
        {
            foreach (var playerInformation in informationList)
            {
                var lastScore = playerInformation.Information.NewInformation.LastScore;
                //playerInformation.Information.NewInformation.MonthPuntuation = playerInformation.Information.OldInformation.MonthPuntuation + lastScore;
                playerInformation.Information.NewInformation.GlobalPuntuation = playerInformation.Information.OldInformation.GlobalPuntuation + lastScore;
            }
        }

        public void AssignPuntuation(IEnumerable<PlayersInformation> informationList)
        {
            var informationListSorted = informationList.OrderByDescending(x => x.Information.NewInformation.GlobalPuntuation);
            for (int i = 0; i < informationListSorted.Count(); i++)
            {
                informationListSorted.ElementAt(i).Information.NewInformation.Position = i + 1;
            }
            foreach (var players in informationList)
            {
                var position =
                    informationListSorted.FirstOrDefault(name => name.PlayerName == players.PlayerName)
                        .Information.NewInformation.Position;
                players.Information.NewInformation.Position = position;
            }
        }

        public bool IsValidPorraAcordingTime(IPublishedContent porraNode, object matchDay = null)
        {
            if (porraNode != null)
            {
                porraNode = Utils.GetMatchNode(porraNode);
                matchDay = porraNode.GetPropertyValue("matchDay");
            }
            var porraTime = DateTime.Parse(matchDay.ToString());
            return porraTime > DateTime.Now;
        }

        public Dictionary<string, BasePorraModel> GetPorrasFromPlayers(IPublishedContent node)
        {
            //node = Utils.GetRootNode(node);
            var nodePlayers = Utils.GetPorresNode(node);
            var porras = new Dictionary<string, BasePorraModel>();
            foreach (var nodePlayer in nodePlayers)
            {
                porras.Add(nodePlayer.Parent.Name, new BasePorraModel
                {
                    LocalTeam = nodePlayer.GetPropertyValue("localTeam") != null ? nodePlayer.GetPropertyValue("localTeam").ToString() : string.Empty,
                    LocalScore = nodePlayer.GetPropertyValue("localScore") != null ? nodePlayer.GetPropertyValue("localScore").ToString() : string.Empty,
                    VisitorTeam = nodePlayer.GetPropertyValue("visitorTeam") != null ? nodePlayer.GetPropertyValue("visitorTeam").ToString() : string.Empty,
                    VisitorScore = nodePlayer.GetPropertyValue("visitorScore") != null ? nodePlayer.GetPropertyValue("visitorScore").ToString() : string.Empty,
                });
            }
            return porras;
        }
    }
}
