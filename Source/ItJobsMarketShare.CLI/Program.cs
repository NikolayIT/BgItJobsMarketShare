namespace ItJobsMarketShare.CLI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using CsQuery;

    using MissingFeatures;
    using System.Web;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class Program
    {
        private static IEnumerable<Technology> technologies = new List<Technology>
        {
            new Technology("C#", new List<string> { "C#", "ASP.NET", "WPF", "WCF", "WinForms", ".NET"}),
            new Technology("Ruby", new List<string> { "Ruby"}),
            new Technology("Python", new List<string> { "Python", "Python", "Django"}),
            new Technology("C/C++", new List<string> { "C++", "C/C++"}),
            new Technology("JavaScript", new List<string> { "JavaScript", "Angular"}),
            new Technology("Delphi", new List<string> { "Delphi"}),
            new Technology("Java", new List<string> { "Java", "JSP", "JEE"}),
            new Technology("Objective-C", new List<string> { "Objective-C", "Objective C", "ObjectiveC"}),
            new Technology("PHP", new List<string> { "PHP", "Zend Framework"}),
            new Technology("VisualBasic", new List<string> { "VisualBasic", "VB.NET"}),
            new Technology("SQL", new List<string> { "SQL", "MSSQL", "MySQL"}),
        };

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var links = GetJobLinks();
            Console.WriteLine("{0} job ads found.", links.Count());

            var languages = new Dictionary<string, int>();

            Parallel.ForEach(links, link =>
            {
                // Console.WriteLine(link);
                var languagesInLink = ParseLanguages(link);
                foreach (var item in languagesInLink)
                {
                    lock (languages)
                    {
                        if (!languages.ContainsKey(item))
                        {
                            languages.Add(item, 0);
                        }

                        languages[item]++;
                    }

                    // Console.WriteLine(item);
                }

                Console.Write(".");
            });

            Console.WriteLine();
            foreach (var item in languages)
            {
                Console.WriteLine("{0} => {1}", item.Key, item.Value);
            }

            Console.ReadLine();
        }

        private static IEnumerable<string> ParseLanguages(string link)
        {
            WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 };
            var webPageContent = webClient.DownloadString(link);
            CQ dom = webPageContent;

            var jobDescription = " " + HttpUtility.HtmlDecode(dom["body > table:nth-child(3) > tbody > tr > td > table > tbody > tr > td:nth-child(1) > table"]
                .Selection.First().InnerHTML.StripHtmlTags()) + " ";
            // Console.WriteLine(jobDescription);
            var languages = new List<string>();

            foreach (var technology in technologies)
            {
                foreach (var term in technology.SearchTerms)
                {
                    var pattern = @"\W" + Regex.Escape(term) + @"\W";
                    if (Regex.IsMatch(jobDescription, pattern, RegexOptions.IgnoreCase))
                    {
                        languages.Add(technology.Name);
                        break;
                    }
                }
            }

            return languages.Distinct();
        }

        private static IEnumerable<string> GetJobLinks()
        {
            WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 };
            const string PageableListUrlFormat = "http://www.jobs.bg/front_job_search.php?frompage={0}&all_cities=0&categories%5B%5D=15&all_type=0&all_position_level=1&all_company_type=1&keyword=#paging";
            const int ItemsPerPage = 15;

            var list = new List<string>();
            int startFrom = 0;
            while (true)
            {
                var webPageContent = webClient.DownloadString(string.Format(PageableListUrlFormat, startFrom));
                CQ dom = webPageContent;
                var links = dom["#search_results .joblink"]
                    .Selection
                    .Select(x => x.Attributes["href"])
                    .Select(x => string.Format("http://www.jobs.bg/{0}", x))
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
