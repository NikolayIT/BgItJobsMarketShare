namespace ItJobsMarketShare
{
    using System.Collections.Generic;

    public class Technology
    {
        public Technology(string name, IEnumerable<string> searchTerms)
        {
            this.Name = name;
            this.SearchTerms = searchTerms;
        }

        public string Name { get; }

        public IEnumerable<string> SearchTerms { get; }
    }
}
