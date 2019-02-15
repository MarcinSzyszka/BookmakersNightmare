using Autofac;

namespace Main.DataManager.Tests.Fixtures
{
    public class BootstrappedFixture
    {
        public IContainer Container { get; }

        public BootstrappedFixture()
        {
            Container = Bootstrapping.Startup.Bootstrap();
        }
    }
}
