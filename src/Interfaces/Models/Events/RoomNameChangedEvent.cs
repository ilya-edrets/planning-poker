namespace PlanningPoker.Interfaces.Models.Events
{
    public record RoomNameChangedEvent(string Name) : RoomEvent
    {
    }
}
