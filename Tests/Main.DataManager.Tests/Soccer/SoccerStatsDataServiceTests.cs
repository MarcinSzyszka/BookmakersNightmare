﻿using System.Threading.Tasks;
using Autofac;
using Main.DataManager.Soccer;
using Main.DataManager.Tests.Fixtures;
using Xunit;

namespace Main.DataManager.Tests.Soccer
{
    public class SoccerStatsDataServiceTests : IClassFixture<BootstrappedFixture>
    {
        [Fact]
        public async Task UpdateResultsData_ShouldUpdateDataCorrectly()
        {
            //Arrange

            //Act
            await _serviceUnderTest.UpdateResultsData("polska", "ekstraklasa");
            //await _serviceUnderTest.UpdateResultsData("hiszpania", "laliga");

            //Assert

        }

        #region CONFIGURATION

        private ISoccerStatsDataService _serviceUnderTest;

        public SoccerStatsDataServiceTests(BootstrappedFixture fixture)
        {
            _serviceUnderTest = fixture.Container.Resolve<ISoccerStatsDataService>();
        }

        #endregion
    }
}
