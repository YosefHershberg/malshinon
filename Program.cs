using Utils;
using DAL;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Malshinon — Community Intel Reporting System\n");
        while (true)
        {
            Console.WriteLine("1. Submit Report");
            Console.WriteLine("2. Import Reports from CSV");
            Console.WriteLine("3. Show Secret Code by Name");
            Console.WriteLine("4. Analysis Dashboard");
            Console.WriteLine("5. Exit");
            Console.Write("Select option: ");
            var opt = Console.ReadLine();
            switch (opt)
            {
                case "1": SubmitReport(); break;
                case "2": ImportCsv(); break;
                case "3": ShowSecretCode(); break;
                case "4": ShowDashboard(); break;
                case "5": return;
                default: Console.WriteLine("Invalid option.\n"); break;
            }
        }
    }

    static void SubmitReport()
    {
        Console.Write("Reporter name or code: ");
        var reporterInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(reporterInput)) { Console.WriteLine("Reporter is required.\n"); return; }
        int reporterId = PeopleDAL.GetOrCreatePerson(reporterInput);

        Console.Write("Target name or code: ");
        var targetInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(targetInput)) { Console.WriteLine("Target is required.\n"); return; }
        int targetId = PeopleDAL.GetOrCreatePerson(targetInput);

        Console.Write("Report text: ");
        var reportText = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(reportText)) { Console.WriteLine("Report text is required.\n"); return; }

        var now = DateTime.Now;
        ReportsDAL.InsertReport(reporterId, targetId, reportText, now);
        Console.WriteLine("Report submitted.\n");
    }

    static void ImportCsv()
    {
        int count = 0;
        using var reader = new StreamReader("./sample_import.csv");
        string? header = reader.ReadLine();
        if (header == null) { Console.WriteLine("CSV is empty.\n"); return; }
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            if (parts.Length < 4) continue;
            var reporter = parts[0];
            var target = parts[1];
            var text = parts[2];
            if (!DateTime.TryParse(parts[3], null, System.Globalization.DateTimeStyles.AssumeLocal, out var ts)) continue;
            if (string.IsNullOrWhiteSpace(reporter) || string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(text)) continue;
            int reporterId = PeopleDAL.GetOrCreatePerson(reporter);
            int targetId = PeopleDAL.GetOrCreatePerson(target);
            ReportsDAL.InsertReport(reporterId, targetId, text, ts);
            count++;
        }
        Logger.Log($"CSVImport: Imported {count} reports from ./sample_import.csv");
    }

    static void ShowSecretCode()
    {
        Console.Write("Full name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Name is required.\n"); return; }
        var code = PeopleDAL.GetSecretCodeByName(name);
        if (string.IsNullOrEmpty(code)) Console.WriteLine("Not found.\n");
        else Console.WriteLine($"Secret code: {code}\n");
    }

    static void ShowDashboard()
    {
        Console.WriteLine("Potential Recruits:");
        var recruits = ReportsDAL.GetPotentialRecruits(); // NOTE: Limit 10
        DBConnection.PrintResult(recruits);
        Console.WriteLine();

        Console.WriteLine("Dangerous Targets:");
        var dangerous = ReportsDAL.GetDangerousTargets(); // NOTE: Limit 10
        DBConnection.PrintResult(dangerous);
        Console.WriteLine();

        Console.WriteLine("Alerts:");
        var alerts = AlertsDAL.GetRecentAlerts(); // NOTE: Limit 10
        DBConnection.PrintResult(alerts);
        Console.WriteLine();
    }
}
