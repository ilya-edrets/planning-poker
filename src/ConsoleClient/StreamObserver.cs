namespace PlanningPoker.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Orleans.Streams;
    using PlanningPoker.Interfaces.Models.Events;

    internal class StreamObserver : IAsyncObserver<RoomEvent>
    {
        private readonly Action<string> printAction;

        public StreamObserver(Action<string> printAction)
        {
            this.printAction = printAction;
        }

        public Task OnCompletedAsync() => Task.CompletedTask;

        public Task OnErrorAsync(Exception ex) => Task.CompletedTask;

        public Task OnNextAsync(RoomEvent item, StreamSequenceToken? token = null)
        {
            var message = item switch
            {
                MemberJoinEvent @event => $"Member {@event.Member.Name} is joined!",
                MemberLeftEvent @event => $"Member {@event.Member.Name} is left.",
                MemberVotedEvent @event => $"Member {@event.Member.Name} set vote{(@event.Vote.HasValue ? " " + @event.Vote.Value : string.Empty)}.",
                RoomNameChangedEvent @event => $"Now this room is {@event.Name}!",
                VotesShowedEvent @event => string.Join(Environment.NewLine, @event.Votes.Select(v => $"{v.Author.Name}: {v.Value}")),
                VotesClearedEvent _ => $"New round is started!",
                _ => throw new NotImplementedException(),
            };

            this.printAction(message);

            return Task.CompletedTask;
        }
    }
}
