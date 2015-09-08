namespace ItJobsMarketShare.CLI
{
    using System.Collections.Generic;

    public class Technology
    {
        public Technology(string name, IEnumerable<string> searchTerms)
        {
            this.Name = name;
            this.SearchTerms = searchTerms;
        }

        public string Name { get; private set; }

        public IEnumerable<string> SearchTerms { get; private set; }
    }
}
