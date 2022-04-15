namespace PlanningPoker.Interfaces.Models.Events
{
    using System.Collections.Generic;

    public record VotesShowedEvent(ICollection<Vote> Votes) : RoomEvent
    {
    }
}
