namespace ItJobsMarketShare.CLI
{
    using System.Collections.Generic;

    public class TechnologiesProvider
    {
        public IEnumerable<Technology> GetTechnologies()
        {
            return new List<Technology>
            {
                new Technology("C#", new List<string> { "C#", "ASP.NET", "WPF", "WCF", "WinForms", ".NET"}),
                new Technology("Ruby", new List<string> { "Ruby"}),
                new Technology("Python", new List<string> { "Python", "Django"}),
                new Technology("C/C++", new List<string> { "C++", "C/C++"}),
                new Technology("JavaScript", new List<string> { "JavaScript", "Angular"}),
                new Technology("Delphi", new List<string> { "Delphi"}),
                new Technology("Java", new List<string> { "Java", "JSP", "JEE"}),
                new Technology("Objective-C", new List<string> { "Objective-C", "Objective C", "ObjectiveC"}),
                new Technology("PHP", new List<string> { "PHP", "Zend Framework"}),
                new Technology("VisualBasic", new List<string> { "VisualBasic", "VB.NET"}),
                new Technology("SQL", new List<string> { "SQL", "MSSQL", "MySQL"}),
            };
        }
    }
}
