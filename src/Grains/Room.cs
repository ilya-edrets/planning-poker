namespace PlanningPoker.Grains
{
    using System;
    using System.Threading.Tasks;
    using Orleans;
    using Orleans.Core;
    using Orleans.Runtime;
    using PlanningPoker.Interfaces.Grains;

    public class Room : Grain, IRoom
    {
        private string name = string.Empty;

        public Task SetName(string name)
        {
            this.name = name;

            return Task.CompletedTask;
        }
    }
}