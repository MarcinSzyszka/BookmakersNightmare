using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using DataRepository.Services.Soccer.Odds;

namespace Main.Bootstrapping
{
    public class Startup
    {
        public static IContainer Bootstrap()
        {
            var containerBuilder = new ContainerBuilder();

            var referencedAssembliesPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var assemblies = referencedAssembliesPaths.Select(path => AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));

            containerBuilder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            containerBuilder.RegisterGeneric(typeof(SoccerOddsRepositoryService<>)).As(typeof(ISoccerOddsRepositoryService<>));

            return containerBuilder.Build();
        }
    }
}
