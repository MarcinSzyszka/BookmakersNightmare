using System;
using Autofac;
using DataRepository.Services.Soccer;

namespace Main.Bootstrapping
{
    public class Startup
    {
        public static IContainer Bootstrap()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            containerBuilder.RegisterGeneric(typeof(SoccerRepositoryService<>)).As(typeof(ISoccerRepositoryService<>));

            return containerBuilder.Build();
        }
    }
}
