namespace PlanningPoker.Interfaces.Models.Events
{
    using System;

    public abstract record RoomEvent
    {
        protected RoomEvent()
        {
            this.Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; }
    }
}
