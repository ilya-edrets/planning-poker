namespace PlanningPoker.Server
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Hosting;
    using PlanningPoker.Grains;

    public class Program
    {
        public static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .UseOrleans(builder =>
                {
                    builder
                        .UseLocalhostClustering()
                        .AddMemoryGrainStorage("PubSubStore")
                        .AddSimpleMessageStreamProvider("room", options =>
                        {
                            options.FireAndForgetDelivery = true;
                        })
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(RoomGrain).Assembly).WithReferences())
                        .UseDashboard();
                })
                .ConfigureLogging(builder => builder.AddConsole())
                .RunConsoleAsync();
        }
    }
}