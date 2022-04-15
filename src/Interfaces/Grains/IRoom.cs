namespace PlanningPoker.Interfaces.Grains
{
    using System.Threading.Tasks;
    using Orleans;

    public interface IRoom : IGrainWithGuidKey
    {
        Task SetName(string name);
    }
}