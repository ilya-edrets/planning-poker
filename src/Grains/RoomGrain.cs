namespace PlanningPoker.Grains
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orleans;
    using Orleans.Streams;
    using PlanningPoker.Common;
    using PlanningPoker.Interfaces.Grains;
    using PlanningPoker.Interfaces.Models;
    using PlanningPoker.Interfaces.Models.Events;

    public class RoomGrain : Grain, IRoomGrain
    {
        private string name = string.Empty;
        private IAsyncStream<RoomEvent> stream = null!;

        private IDictionary<Guid, Member> members = null!;
        private IDictionary<Guid, Vote> votes = null!;
        private bool isVotesVisible;

        public override Task OnActivateAsync()
        {
            var streamProvider = this.GetStreamProvider("room");
            this.stream = streamProvider.GetStream<RoomEvent>(Guid.NewGuid(), "default");

            this.members = new Dictionary<Guid, Member>();
            this.votes = new Dictionary<Guid, Vote>();

            return base.OnActivateAsync();
        }

        public Task SetName(string name)
        {
            this.name = name;

            var @event = new RoomNameChangedEvent(name);
            return this.stream.OnNextAsync(@event);
        }

        public Task<string> GetName()
        {
            return this.name.AsTask();
        }

        public async Task<Guid> Join(Member member)
        {
            if (!this.members.ContainsKey(member.Id))
            {
                this.members.Add(member.Id, member);
                var @event = new MemberJoinEvent(member);
                await this.stream.OnNextAsync(@event);
            }

            return this.stream.Guid;
        }

        public async Task Leave(Member member)
        {
            if (this.members.ContainsKey(member.Id))
            {
                this.members.Remove(member.Id);
                var @event = new MemberLeftEvent(member);
                await this.stream.OnNextAsync(@event);
            }
        }

        public Task<ICollection<Member>> GetMembers()
        {
            return this.members.Values.AsTask();
        }

        public Task Vote(Vote vote)
        {
            if (this.votes.ContainsKey(vote.Author.Id))
            {
                this.votes[vote.Author.Id] = vote;
            }
            else
            {
                this.votes.Add(vote.Author.Id, vote);
            }

            var @event = new MemberVotedEvent(vote.Author, this.isVotesVisible ? vote.Value : null);
            return this.stream.OnNextAsync(@event);
        }

        public Task ShowVotes()
        {
            this.isVotesVisible = true;

            var @event = new VotesShowedEvent(this.votes.Values);
            return this.stream.OnNextAsync(@event);
        }

        public Task ClearVotes()
        {
            this.isVotesVisible = true;
            this.votes.Clear();

            var @event = new VotesClearedEvent();
            return this.stream.OnNextAsync(@event);
        }
    }
}