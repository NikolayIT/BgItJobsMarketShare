namespace ItJobsMarketShare.CLI
{
    using System.Collections.Generic;

    public class TechnologiesProvider
    {
        public IEnumerable<Technology> GetTechnologies()
        {
            return new List<Technology>
            {
                new Technology("C#", new List<string> { "C#", "ASP.NET", "WPF", "WCF", "WinForms", ".NET" }),
                new Technology("Ruby", new List<string> { "Ruby" }),
                new Technology("Python", new List<string> { "Python", "Django" }),
                new Technology("Perl", new List<string> { "Perl" }),
                new Technology("C/C++", new List<string> { "C++", "C/C++" }),
                new Technology("JavaScript", new List<string> { "JavaScript", "Angular" }),
                new Technology("HTML/CSS", new List<string> { "HTML", "CSS" }),
                new Technology("Delphi/Pascal", new List<string> { "Delphi", "Pascal" }),
                new Technology("Java", new List<string> { "Java", "JSP", "JEE" }),
                new Technology("Objective-C", new List<string> { "Objective-C", "Objective C", "ObjectiveC" }),
                new Technology("Android", new List<string> { "Android" }),
                new Technology("Windows Phone", new List<string> { "Windows Phone", "Windows Mobile", "WinPhone" }),
                new Technology("Swift", new List<string> { "Swift" }),
                new Technology("PHP", new List<string> { "PHP", "Zend Framework" }),
                new Technology("VisualBasic", new List<string> { "VisualBasic", "VB.NET" }),
                new Technology("SQL", new List<string> { "SQL", "MSSQL", "MySQL" }),
                new Technology("Sharepoint", new List<string> { "Sharepoint" }),
                new Technology("Unity", new List<string> { "Unity", "Unity3d" }),
            };
        }
    }
}
