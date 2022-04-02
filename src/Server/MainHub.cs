namespace PlanningPoker.Server
{
    using Microsoft.AspNetCore.SignalR;
    using PlanningPoker.Interfaces.Hubs;

    public class MainHub : Hub<IHubClient>, IHubServer
    {
    }
}
