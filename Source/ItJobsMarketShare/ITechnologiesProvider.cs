namespace ItJobsMarketShare
{
    using System.Collections.Generic;

    public interface ITechnologiesProvider
    {
        IEnumerable<Technology> GetTechnologies();
    }
}
