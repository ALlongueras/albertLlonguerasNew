using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.Businness;
using Library.Models;
using NUnit.Framework;
using Moq;

namespace Library.Test.Businness
{
    [TestFixture]
    public class PorraManagerTest
    {
        private List<PlayersInformation> informationList;
        private MatchResultModel matchResult;
        private Dictionary<string, BasePorraModel> porras;

        private IPorraManager porraManager;

        [SetUp]
        public void Initialiaze()
        {
            informationList = new List<PlayersInformation>();
            informationList = this.InitialiazePlayersInformation(this.informationList);
            matchResult = new MatchResultModel();
            matchResult = this.InitializeMatchResult(matchResult);
            porras=new Dictionary<string, BasePorraModel>();
            porras = this.InitializePorras(porras);
            porraManager = new PorraManager();
        }

        public MatchResultModel InitializeMatchResult(MatchResultModel matchResult)
        {
            matchResult.FinalOfMonth = true;
            //matchResult.LocalScore = "5,3";
            //matchResult.VisitorScore = "12";
            return matchResult;
        }

        public Dictionary<string, BasePorraModel> InitializePorras(Dictionary<string, BasePorraModel> porras)
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

        public List<PlayersInformation> InitialiazePlayersInformation(List<PlayersInformation> information)
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

        [Test]
        public void CheckDRSOfPlayers()
        {
            informationList = this.porraManager.AssignDRSToPlayers(this.informationList);
            Assert.AreEqual(false, informationList[0].Information.NewInformation.HasDRS);
            Assert.AreEqual(true, informationList[1].Information.NewInformation.HasDRS);
            Assert.AreEqual(true, informationList[2].Information.NewInformation.HasDRS);
        }

        [Test]
        public void CheckFinalOfMonth()
        {
            this.porraManager.FinalOfMonth(this.informationList, this.matchResult);
            Assert.AreEqual((decimal)3.5, informationList[0].Information.NewInformation.PorreroPuntuation);
            Assert.AreEqual((decimal)2.5, informationList[1].Information.NewInformation.PorreroPuntuation);
            Assert.AreEqual((decimal)4, informationList[2].Information.NewInformation.PorreroPuntuation);
        }

        [Test]
        public void UpdateNewScoreRegardingGlobalPuntuation()
        {
            informationList[0].Information.NewInformation.LastScore = 3;
            informationList[1].Information.NewInformation.LastScore = 2;
            informationList[2].Information.NewInformation.LastScore = 5;
            this.porraManager.UpdateNewScore(this.informationList);
            Assert.AreEqual(13, this.informationList[0].Information.NewInformation.GlobalPuntuation);
            Assert.AreEqual(10, this.informationList[1].Information.NewInformation.GlobalPuntuation);
            Assert.AreEqual(12, this.informationList[2].Information.NewInformation.GlobalPuntuation);
        }

        [Test]
        public void UpdateNewScoreRegardingMonthPuntuation()
        {
            informationList[0].Information.NewInformation.LastScore = 3;
            informationList[1].Information.NewInformation.LastScore = 2;
            informationList[2].Information.NewInformation.LastScore = 5;
            this.porraManager.UpdateNewScore(this.informationList);
            Assert.AreEqual((decimal)9.5, this.informationList[0].Information.NewInformation.MonthPuntuation);
            Assert.AreEqual((decimal)8.5, this.informationList[1].Information.NewInformation.MonthPuntuation);
            Assert.AreEqual((decimal)11.3, this.informationList[2].Information.NewInformation.MonthPuntuation);
        }

        [TestCase("5,3", "1", 4.0, 1.0, 0.0)]
        [TestCase("5,3,1", "12", 1.5, 4.0, 0.0)]
        [TestCase("20,5,4", "12", 1.5, 5.0, 0.0)]
        [TestCase("5,3", "", 2.0, 0.0, 0.0)]
        [TestCase("", "2", 1.0, 1.0, 0.0)]
        [TestCase("5,3,4", "3", 1.0, 4.0, 0.0)]
        [TestCase("", "12", 1.5, 1.5, 0.0)]
        public void UpdateResults(string localMatchResult, string visitoMatchResult, decimal result1, decimal result2, decimal result3)
        {
            matchResult.LocalScore = localMatchResult;
            matchResult.VisitorScore = visitoMatchResult;
            this.porraManager.UpdateResults(this.porras, this.informationList, this.matchResult);
            Assert.AreEqual(result1, this.informationList[0].Information.NewInformation.LastScore);
            Assert.AreEqual(result2, this.informationList[1].Information.NewInformation.LastScore);
            Assert.AreEqual(result3, this.informationList[2].Information.NewInformation.LastScore);
        }

        [TestCase("5,3", "1", 4.0, 1.5, 0.0)]
        [TestCase("5,3,1", "12", 1.5, 6.0, 0.0)]
        [TestCase("20,5,4", "12", 1.5, 7.5, 0.0)]
        [TestCase("5,3", "", 2.0, 0.0, 0.0)]
        [TestCase("", "2", 1.0, 1.5, 0.0)]
        [TestCase("5,3,4", "3", 1.0, 6.0, 0.0)]
        [TestCase("", "12", 1.5, 2.25, 0.0)]
        public void UpdateResultsWithDRSToPlayer2(string localMatchResult, string visitoMatchResult, decimal result1, decimal result2, decimal result3)
        {
            matchResult.LocalScore = localMatchResult;
            matchResult.VisitorScore = visitoMatchResult;
            this.informationList[1].Information.NewInformation.HasDRS = true;
            this.porraManager.UpdateResults(this.porras, this.informationList, this.matchResult);
            this.porraManager.ApplyDRSToPlayers(this.informationList);
            Assert.AreEqual(result1, this.informationList[0].Information.NewInformation.LastScore);
            Assert.AreEqual(result2, this.informationList[1].Information.NewInformation.LastScore);
            Assert.AreEqual(result3, this.informationList[2].Information.NewInformation.LastScore);
        }
    }
}
