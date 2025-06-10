using Utils;

namespace DAL
{
    public static class ReportsDAL
    {
        public static void InsertReport(int reporterId, int targetId, string reportText, DateTime submittedAt)
        {
            string sql = $"INSERT INTO Reports (ReporterId, TargetId, ReportText, SubmittedAt) VALUES ('{reporterId}', '{targetId}', '{reportText.Replace("'", "''")}', '{submittedAt:yyyy-MM-dd HH:mm:ss}')";
            DBConnection.Execute(sql);
            AlertsDAL.CheckAndTriggerAlerts(targetId);
            Logger.Log($"ReportSubmission: Reporter {reporterId} -> Target {targetId}");
        }

        public static List<Dictionary<string, object?>> GetPotentialRecruits()
        {
            string sql = @"
                SELECT p.FullName, COUNT(r.Id) as ReportCount, AVG(CHAR_LENGTH(r.ReportText)) as AvgLen
                FROM People p JOIN Reports r ON p.Id = r.ReporterId
                GROUP BY p.Id HAVING ReportCount >= 10 AND AvgLen >= 100";
            return DBConnection.Execute(sql);
        }

        public static List<Dictionary<string, object?>> GetDangerousTargets()
        {
            string sql = @"
                SELECT p.FullName, COUNT(r.Id) as Mentions
                FROM People p JOIN Reports r ON p.Id = r.TargetId
                GROUP BY p.Id HAVING Mentions >= 20";
            return DBConnection.Execute(sql);
        }
    }
}