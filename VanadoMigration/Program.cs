using DbUp;
using System.Reflection;

namespace VanadoMigration
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString = "Host=localhost;Port=5432;Database=Vanado_Test;Username=postgres;Password=postgres";

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Migracije uspješno izvršene.");
            Console.ResetColor();

            return 0;
        }
    }
}
