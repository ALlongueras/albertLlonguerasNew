using System.Collections.Generic;
using Library.Businness;
using Library.Models;
using NUnit.Framework;

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
            informationList = PorraManagerInitializers.InitialiazePlayersInformation(this.informationList);
            matchResult = new MatchResultModel();
            matchResult = PorraManagerInitializers.InitializeMatchResult(matchResult);
            porras = new Dictionary<string, BasePorraModel>();
            porras = PorraManagerInitializers.InitializePorras(porras);
            porraManager = new PorraManager();
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
        [TestCase("20,5, 4", "12", 1.5, 7.5, 0.0)]
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

        [Test]
        public void SortPlayers()
        {
            this.informationList[2].Information.NewInformation.GlobalPuntuation = 12;
            this.porraManager.AssignPuntuation(this.informationList);
            Assert.AreEqual(2, this.informationList[0].Information.NewInformation.Position);
            Assert.AreEqual(3, this.informationList[1].Information.NewInformation.Position);
            Assert.AreEqual(1, this.informationList[2].Information.NewInformation.Position);
        }

        [TestCase("8/26/2014 12:00:00 AM", true)]
        [TestCase("8/25/2014 12:00:00 AM", true)]
        [TestCase("8/24/2014 12:00:00 AM", false)]
        public void CheckIfPorraIsValidAcordingTime(string timeMock, bool expectedResult)
        {
            var result = this.porraManager.IsValidPorraAcordingTime(null, timeMock);
            Assert.AreEqual(result, expectedResult);
        }
    }
}
