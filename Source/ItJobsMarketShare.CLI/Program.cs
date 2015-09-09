namespace ItJobsMarketShare.CLI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    using CsQuery;

    using MissingFeatures;

    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var links = GetJobLinks().ToList();
            Console.WriteLine("{0} job ads found.", links.Count);

            var technologiesList = new Dictionary<string, int>();

            Parallel.ForEach(links, link =>
            {
                var technologiesInLink = ParseTechnologies(link);
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

                Console.Write(".");
            });

            Console.WriteLine();
            Console.WriteLine("Technology,Count");
            foreach (var item in technologiesList.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"{item.Key},{item.Value}");
            }
        }

        private static IEnumerable<string> ParseTechnologies(string link)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var webPageContent = webClient.DownloadString(link);
            CQ dom = webPageContent;

            var jobDescription = " " + HttpUtility.HtmlDecode(dom["body > table:nth-child(3) > tbody > tr > td > table > tbody > tr > td:nth-child(1) > table"]
                .Selection.First().InnerHTML.StripHtmlTags()) + " ";

            var technologies = new List<string>();

            foreach (var technology in new TechnologiesProvider().GetTechnologies())
            {
                foreach (var term in technology.SearchTerms)
                {
                    var pattern = @"\W" + Regex.Escape(term) + @"\W";
                    if (Regex.IsMatch(jobDescription, pattern, RegexOptions.IgnoreCase))
                    {
                        technologies.Add(technology.Name);
                        break;
                    }
                }
            }

            return technologies.Distinct();
        }

        private static IEnumerable<string> GetJobLinks()
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            const string PageableListUrlFormat = "http://www.jobs.bg/front_job_search.php?frompage={0}&all_cities=0&categories%5B%5D=15&all_type=0&all_position_level=1&all_company_type=1&keyword=#paging";
            const int ItemsPerPage = 15;

            var list = new List<string>();
            var startFrom = 0;
            while (true)
            {
                var webPageContent = webClient.DownloadString(string.Format(PageableListUrlFormat, startFrom));
                CQ dom = webPageContent;
                var links = dom["#search_results .joblink"]
                    .Selection
                    .Select(x => x.Attributes["href"])
                    .Select(x => $"http://www.jobs.bg/{x}")
                    .ToList();
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
