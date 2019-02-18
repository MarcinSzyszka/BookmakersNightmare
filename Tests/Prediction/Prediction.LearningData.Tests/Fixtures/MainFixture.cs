using Autofac;
using Main.Bootstrapping;

namespace Prediction.LearningData.Tests.Fixtures
{
    public class MainFixture
    {
        public IContainer Container { get; }

        public MainFixture()
        {
            Container = Startup.Bootstrap();
        }
    }
}
