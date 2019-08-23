namespace ItJobsMarketShare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using AngleSharp;
    using AngleSharp.Html.Parser;

    using Ganss.XSS;

    public class JobsBgTechnologiesCountProvider
    {
        private readonly IHtmlParser parser = new HtmlParser();
        private readonly IHtmlSanitizer sanitizer = new HtmlSanitizer();

        public Dictionary<string, int> GetCount(IEnumerable<Technology> technologies)
        {
            var technologiesList = new Dictionary<string, int>();
            var links = this.GetJobLinks().ToList();
            Console.WriteLine("{0} job ads found.", links.Count);

            Parallel.ForEach(
                links,
                link =>
                    {
                        var technologiesInLink = this.ParseTechnologies(link, technologies);
                        foreach (var item in technologiesInLink)
                        {
                            lock (technologiesList)
                            {
                                if (!technologiesList.ContainsKey(item))
                                {
                                    technologiesList.Add(item, 0);
                                }

                                technologiesList[item]++;
                            }
                        }

                        Thread.Sleep(10000);
                        Console.Write(".");
                    });

            return technologiesList;
        }

        public IEnumerable<string> ParseTechnologies(string link, IEnumerable<Technology> technologies)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var webPageContent = webClient.DownloadString(link);
            var document = this.parser.ParseDocument(webPageContent);

            var jobDescription = " " + Regex.Replace(
                                     HttpUtility.HtmlDecode(
                                         this.sanitizer.Sanitize(
                                             document.QuerySelector("table[style*=\"width:980px\"]")?.ToHtml())
                                         ?? string.Empty),
                                     "<.*?>",
                                     string.Empty) + " ";

            var technologiesFound = new List<string>();

            foreach (var technology in technologies)
            {
                foreach (var term in technology.SearchTerms)
                {
                    var pattern = $@"\W{Regex.Escape(term)}\W";
                    if (Regex.IsMatch(jobDescription, pattern, RegexOptions.IgnoreCase))
                    {
                        technologiesFound.Add(technology.Name);
                        break;
                    }
                }
            }

            return technologiesFound.Distinct();
        }

        private IEnumerable<string> GetJobLinks()
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            const string PageableListUrlFormat = "http://www.jobs.bg/front_job_search.php?frompage={0}&all_cities=0&categories%5B%5D=15&all_type=0&all_position_level=1&all_company_type=1&keyword=#paging";
            const int ItemsPerPage = 15;

            var list = new List<string>();
            var startFrom = 0;
            while (true)
            {
                var webPageContent = webClient.DownloadString(string.Format(PageableListUrlFormat, startFrom));
                var document = this.parser.ParseDocument(webPageContent);
                var links = document.QuerySelectorAll("#search_results_div .joblink").Select(x => x.Attributes["href"].Value)
                    .Select(x => $"http://www.jobs.bg/{x}").ToList();
                list.AddRange(links);

                startFrom += ItemsPerPage;

                Console.Write(".");

                if (links.Count < ItemsPerPage)
                {
                    break;
                }
            }

            Console.WriteLine();

            return list;
        }
    }
}
