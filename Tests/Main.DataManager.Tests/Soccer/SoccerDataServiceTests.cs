﻿using System.Threading.Tasks;
using Autofac;
using Main.DataManager.Soccer.Odds.Services;
using Main.DataManager.Tests.Fixtures;
using Main.Infrastructure.Enums;
using Xunit;

namespace Main.DataManager.Tests.Soccer
{
    public class SoccerDataServiceTests : IClassFixture<BootstrappedFixture>
    {
        [Fact]
        public async Task UpdateResultsData_ShouldDownloadDataAndStoreItInDb()
        {
            //Arrange

            //Act
            await _serviceUnderTest.UpdateResultsData(Country.Gambia, "gfa-league");

            //Assert
        }
        #region CONFIGURATION

        private readonly ISoccerOddsDataService _serviceUnderTest;
        private readonly BootstrappedFixture _fixture;

        public SoccerDataServiceTests(BootstrappedFixture fixture)
        {
            _fixture = fixture;
            _serviceUnderTest = fixture.Container.Resolve<ISoccerOddsDataService>();
        }
        #endregion
    }
}
