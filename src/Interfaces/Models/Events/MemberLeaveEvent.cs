namespace PlanningPoker.Interfaces.Models.Events
{
    public record MemberLeaveEvent(Member Member) : RoomEvent
    {
    }
}
