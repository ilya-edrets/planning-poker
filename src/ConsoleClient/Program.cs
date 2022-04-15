namespace PlanningPoker.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Orleans;
    using Orleans.Hosting;
    using Orleans.Streams;
    using PlanningPoker.Common;
    using PlanningPoker.Interfaces.Grains;
    using PlanningPoker.Interfaces.Models;
    using PlanningPoker.Interfaces.Models.Events;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IRoomGrain).Assembly).WithReferences())
                .AddSimpleMessageStreamProvider("room")
                .Build();

            await client.Connect();

            var memberId = Guid.NewGuid();
            var name = GetName();
            var roomId = GetRoomId();

            var me = new Member(memberId, name);

            var roomGrain = client.GetGrain<IRoomGrain>(roomId);
            var streamId = await roomGrain.Join(me);
            var subscription = await ConnectToStream(client, streamId);

            await HandleUserInput(roomGrain, me);

            await subscription.UnsubscribeAsync();
        }

        private static async Task HandleUserInput(IRoomGrain room, Member me)
        {
            while (true)
            {
                var input = Console.ReadLine()?.ToLower();

                switch (input)
                {
                    case null:
                        continue;

                    case "leave":
                        await room.Leave(me);
                        return;

                    case var s when s.StartsWith("set room name"):
                        var name = s.Replace("set room name", string.Empty).Trim();
                        await room.SetName(name);
                        break;

                    case "get room name":
                        Console.WriteLine(await room.GetName());
                        break;

                    case "get members":
                        Console.WriteLine((await room.GetMembers()).Select(m => m.Name).JoinStrings(", "));
                        break;

                    case var s when s.StartsWith("vote"):
                        var voteStr = s.Replace("vote", string.Empty).Trim();
                        if (int.TryParse(voteStr, out var voteValue) && voteValue >= 0)
                        {
                            await room.Vote(new Vote(me, voteValue));
                        }
                        else
                        {
                            Console.WriteLine("Invalid vote value");
                        }

                        break;

                    case "show votes":
                        await room.ShowVotes();
                        break;

                    case "clear votes":
                        await room.ClearVotes();
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        private static string GetName()
        {
            string? name;
            do
            {
                Console.Write("Enter your name: ");
                name = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(name));

            return name;
        }

        private static Guid GetRoomId()
        {
            Guid roomId;
            do
            {
                Console.Write("Enter room id: ");
            }
            while (!Guid.TryParse(Console.ReadLine(), out roomId));

            return roomId;
        }

        private static Task<StreamSubscriptionHandle<RoomEvent>> ConnectToStream(IClusterClient client, Guid streamId)
        {
            var stream = client.GetStreamProvider("room")
                .GetStream<RoomEvent>(streamId, "default");

            return stream.SubscribeAsync(new StreamObserver(Console.WriteLine));
        }
    }
}
