namespace PlanningPoker.Interfaces.Models.Events
{
    public record MemberVotedEvent(Member Member, int? vote) : RoomEvent
    {
    }
}
