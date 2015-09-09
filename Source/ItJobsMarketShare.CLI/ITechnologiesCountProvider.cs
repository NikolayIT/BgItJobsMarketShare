namespace ItJobsMarketShare.CLI
{
    using System.Collections.Generic;

    public interface ITechnologiesCountProvider
    {
        Dictionary<string, int> GetCount(IEnumerable<Technology> technologies);
    }
}
