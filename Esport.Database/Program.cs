using System.Reflection;
using DbUp;

internal class Program
{
    private static int Main(string[] args)
    {
        Console.WriteLine("Running DbUp!");

        var connectionString =
            args.FirstOrDefault()
            ?? "Server=esport_mssql; Database=Esport; User Id=sa; Password=Z-PQW9aCvSE!nhg; TrustServerCertificate=True";

        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithVariablesDisabled()
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
        Console.WriteLine("Success!");
        Console.ResetColor();
        return 0;
    }
}