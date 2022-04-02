namespace PlanningPoker.Grains
{
    using Orleans;
    using PlanningPoker.GrainInterfaces;

    public class User : Grain, IUser
    {
    }
}