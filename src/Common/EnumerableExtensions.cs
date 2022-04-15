namespace PlanningPoker.Common
{
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        public static string JoinStrings(this IEnumerable<string> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }
    }
}