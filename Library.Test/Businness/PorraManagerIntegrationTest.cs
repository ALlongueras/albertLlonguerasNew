using System.Collections.Generic;
using Library.Businness;
using Library.Models;
using Moq;
using NUnit.Framework;
//using Umbraco.Core.Models;

namespace Library.Test.Businness
{
    [TestFixture]
    public class PorraManagerIntegrationTest
    {
        private List<PlayersInformation> informationList;
        private MatchResultModel matchResult;
        private Dictionary<string, BasePorraModel> porras;

        private Mock<IPorraManager> porraManagerMock; 

        private IPorraManager porraManager;

        [SetUp]
        public void Init()
        {
            porraManagerMock = new Mock<IPorraManager>(MockBehavior.Strict);
            informationList = new List<PlayersInformation>();
            informationList = PorraManagerInitializers.InitialiazePlayersInformation(this.informationList);
            matchResult = new MatchResultModel();
            matchResult = PorraManagerInitializers.InitializeMatchResult(matchResult);
            porras = new Dictionary<string, BasePorraModel>();
            porras = PorraManagerInitializers.InitializePorras(porras);
            porraManager = new PorraManager();
        }

        //[TestCase("5,3", "1", 19.0, 9.5, 7.0)]
        [TestCase("5,3,1", "12", 16.5, 14.0, 7.0)]
        [TestCase("20,5,4", "12", 14.0, 18.0, 7.0)]
        [TestCase("5,3", "", 17.0, 8.0, 7.0)]
        [TestCase("", "2", 16.0, 9.5, 7.0)]
        [TestCase("5,3,4", "3", 16.0, 14.0, 7.0)]
        [TestCase("", "10", 13.5, 11.5, 7.0)]
        public void GetWholePuntuationRegardingGlobalPuntuation(string localMatchResult, string visitoMatchResult, decimal result1, decimal result2, decimal result3)
        {
            this.informationList[1].Information.OldInformation.MonthPuntuation = 3;
            this.matchResult.LocalScore = localMatchResult;
            this.matchResult.VisitorScore = visitoMatchResult;

            informationList = this.porraManager.AssignDRSToPlayers(informationList);
            informationList = this.porraManager.UpdateResults(porras, informationList, matchResult);
            this.porraManager.ApplyDRSToPlayers(informationList);
            this.porraManager.FinalOfMonth(informationList, matchResult);
            this.porraManager.UpdateNewScore(informationList);

            Assert.AreEqual(result1, this.informationList[0].Information.NewInformation.GlobalPuntuation);
            Assert.AreEqual(result2, this.informationList[1].Information.NewInformation.GlobalPuntuation);
            Assert.AreEqual(result3, this.informationList[2].Information.NewInformation.GlobalPuntuation);
        }

        [TestCase("5,3", "1", 1, 2, 3)]
        [TestCase("5,3,1", "12", 2, 1, 3)]
        [TestCase("20,5,4", "12", 2, 1, 3)]
        [TestCase("5,3", "", 1, 2, 3)]
        [TestCase("", "2", 1, 2, 3)]
        [TestCase("5,3,4", "3", 2, 1, 3)]
        [TestCase("", "12", 1, 2, 3)]
        public void GetWholePuntuationRegardingPosition(string localMatchResult, string visitoMatchResult, decimal result1, decimal result2, decimal result3)
        {
            this.informationList[0].Information.OldInformation.MonthPuntuation = 7;
            this.matchResult.LocalScore = localMatchResult;
            this.matchResult.VisitorScore = visitoMatchResult;
            this.matchResult.FinalOfMonth = false;
            this.matchResult.NotDRS = false;

            informationList = this.porraManager.AssignDRSToPlayers(informationList);
            informationList = this.porraManager.UpdateResults(porras, informationList, matchResult);
            this.porraManager.ApplyDRSToPlayers(informationList);
            this.porraManager.FinalOfMonth(informationList, matchResult);
            this.porraManager.UpdateNewScore(informationList);
            this.porraManager.AssignPuntuation(this.informationList);

            Assert.AreEqual(result1, this.informationList[0].Information.NewInformation.Position);
            Assert.AreEqual(result2, this.informationList[1].Information.NewInformation.Position);
            Assert.AreEqual(result3, this.informationList[2].Information.NewInformation.Position);
        }

        [TestCase("5,3", "1", 14.0, 9.5, 7.0)]
        [TestCase("5,3,1", "12", 11.5, 14.0, 7.0)]
        [TestCase("20,5,4", "12", 11.5, 15.5, 7.0)]
        [TestCase("5,3", "", 12.0, 8.0, 7.0)]
        [TestCase("", "2", 11.0, 9.5, 7.0)]
        [TestCase("5,3,4", "3", 11.0, 14.0, 7.0)]
        [TestCase("", "12", 11.5, 10.25, 7.0)]
        public void GetWholePuntuationRegardingGlobalPuntuationWithFinalOfMonth(string localMatchResult, string visitoMatchResult, decimal result1, decimal result2, decimal result3)
        {
            this.matchResult.LocalScore = localMatchResult;
            this.matchResult.VisitorScore = visitoMatchResult;
            this.matchResult.FinalOfMonth = true;

            informationList = this.porraManager.AssignDRSToPlayers(informationList);
            informationList = this.porraManager.UpdateResults(porras, informationList, matchResult);
            this.porraManager.ApplyDRSToPlayers(informationList);
            this.porraManager.FinalOfMonth(informationList, matchResult);
            this.porraManager.UpdateNewScore(informationList);

            Assert.AreEqual(result1, this.informationList[0].Information.NewInformation.GlobalPuntuation);
            Assert.AreEqual(result2, this.informationList[1].Information.NewInformation.GlobalPuntuation);
            Assert.AreEqual(result3, this.informationList[2].Information.NewInformation.GlobalPuntuation);
        }

        [TestCase("5,3", "1", 1, 2, 3)]
        [TestCase("5,3,1", "12", 2, 1, 3)]
        [TestCase("20,5,4", "12", 2, 1, 3)]
        [TestCase("5,3", "", 1, 2, 3)]
        [TestCase("", "2", 1, 2, 3)]
        [TestCase("5,3,4", "3", 2, 1, 3)]
        [TestCase("", "12", 1, 2, 3)]
        public void GetWholePuntuationRegardingPositionWithFinalOfMonth(string localMatchResult, string visitoMatchResult, decimal result1, decimal result2, decimal result3)
        {
            this.matchResult.LocalScore = localMatchResult;
            this.matchResult.VisitorScore = visitoMatchResult;
            this.matchResult.FinalOfMonth = false;

            informationList = this.porraManager.AssignDRSToPlayers(informationList);
            informationList = this.porraManager.UpdateResults(porras, informationList, matchResult);
            this.porraManager.ApplyDRSToPlayers(informationList);
            this.porraManager.FinalOfMonth(informationList, matchResult);
            this.porraManager.UpdateNewScore(informationList);
            this.porraManager.AssignPuntuation(this.informationList);

            Assert.AreEqual(result1, this.informationList[0].Information.NewInformation.Position);
            Assert.AreEqual(result2, this.informationList[1].Information.NewInformation.Position);
            Assert.AreEqual(result3, this.informationList[2].Information.NewInformation.Position);
        }
    }
}
