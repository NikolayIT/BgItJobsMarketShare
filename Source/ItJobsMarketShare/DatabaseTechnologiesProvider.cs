namespace ItJobsMarketShare
{
    using System.Collections.Generic;

    public class DatabaseTechnologiesProvider : ITechnologiesProvider
    {
        public IEnumerable<Technology> GetTechnologies()
        {
            return new List<Technology>
            {
                new Technology("MSSQL", new List<string> { "Microsoft SQL", "MSSQL", "SQL Server" }),
                new Technology("Oracle", new List<string> { "Oracle" }),
                new Technology("MySQL", new List<string> { "MySQL" }),
                new Technology("MongoDb", new List<string> { "MongoDB", "Mongo" }),
                new Technology("PostgreSQL", new List<string> { "PostgreSQL", "Postgre" }),
                new Technology("SQLite", new List<string> { "SQLite" }),
                new Technology("Redis", new List<string> { "Redis" }),
            };
        }
    }
}
