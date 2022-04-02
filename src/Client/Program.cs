namespace PlanningPoker.Client
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Hosting;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .UseSignalR()
                .Build();

            await client.Connect();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    services =>
                    {
                        services.AddSingleton<IClusterClient>(client)
                            .AddSignalR()
                            .AddOrleans();
                    })
                .Build();

            await host.RunAsync();
            await host.WaitForShutdownAsync();
        }
    }
}
