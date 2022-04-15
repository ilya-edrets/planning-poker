namespace PlanningPoker.Interfaces.Models.Events
{
    public record MemberLeftEvent(Member Member) : RoomEvent
    {
    }
}
