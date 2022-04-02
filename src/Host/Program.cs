namespace PlanningPoker.Host
{
    using System.Threading.Tasks;
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
                        .UseDashboard(configuration => configuration.Port = 3333) // default port is 8080
                        .UseLocalhostClustering();
                })
                .Build()
                .RunAsync();
        }
    }
}