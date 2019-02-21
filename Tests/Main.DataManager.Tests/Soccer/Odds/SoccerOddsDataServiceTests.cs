using System.Threading.Tasks;
using Autofac;
using Main.DataManager.Soccer.Odds;
using Main.DataManager.Tests.Fixtures;
using Main.Infrastructure.Enums;
using Xunit;

namespace Main.DataManager.Tests.Soccer.Odds
{
    public class SoccerOddsDataServiceTests : IClassFixture<BootstrappedFixture>
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

        public SoccerOddsDataServiceTests(BootstrappedFixture fixture)
        {
            _fixture = fixture;
            _serviceUnderTest = fixture.Container.Resolve<ISoccerOddsDataService>();
        }
        #endregion
    }
}
