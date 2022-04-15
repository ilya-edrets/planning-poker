namespace PlanningPoker.Interfaces.Models.Events
{
    public record MemberJoinEvent(Member Member) : RoomEvent
    {
    }
}
