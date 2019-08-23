namespace ItJobsMarketShare
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            ITechnologiesProvider technologiesProvider = new CommonTechnologiesProvider();
            var technologiesCountProvider = new JobsBgTechnologiesCountProvider();

            var technologiesList = technologiesCountProvider.GetCount(technologiesProvider.GetTechnologies());
            Console.WriteLine();

            using (var writer = new StreamWriter($"{DateTime.UtcNow:yyyy-MM-dd}.csv"))
            {
                writer.WriteLine("Technology,Count");
                Console.WriteLine("Technology,Count");
                foreach (var item in technologiesList.OrderByDescending(x => x.Value))
                {
                    writer.WriteLine($"{item.Key},{item.Value}");
                    Console.WriteLine($"{item.Key},{item.Value}");
                }
            }
        }
    }
}
