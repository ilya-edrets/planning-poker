namespace PlanningPoker.ConsoleClient
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Orleans;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .Build();

            await client.Connect();

            var host = Host.CreateDefaultBuilder(args)
                .Build();

            await host.RunAsync();
            await host.WaitForShutdownAsync();
        }
    }
}
