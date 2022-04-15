namespace PlanningPoker.Server
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Orleans;
    using Orleans.Hosting;

    public class Program
    {
        public static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseOrleans(builder =>
                {
                    builder
                        .AddMemoryGrainStorage("PubSubStore")
                        .UseDashboard(configuration => configuration.Port = 3333) // default port is 8080
                        .UseLocalhostClustering();
                })
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Build()
                .RunAsync();
        }
    }
}