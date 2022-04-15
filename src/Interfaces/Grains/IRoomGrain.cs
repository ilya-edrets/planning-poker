namespace PlanningPoker.Interfaces.Grains
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orleans;
    using PlanningPoker.Interfaces.Models;

    public interface IRoomGrain : IGrainWithGuidKey
    {
        Task SetName(string name);

        Task<string> GetName();

        Task<Guid> Join(Member member);

        Task Leave(Member member);

        Task<ICollection<Member>> GetMembers();

        Task Vote(Vote vote);

        Task ShowVotes();

        Task ClearVotes();
    }
}