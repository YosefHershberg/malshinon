using Utils;

namespace DAL
{
    public static class AlertsDAL
    {
        public static void CheckAndTriggerAlerts(int targetId)
        {
            // Check for 20+ mentions
            string sql = $"SELECT COUNT(*) as Mentions FROM Reports WHERE TargetId={targetId}";
            var result = DBConnection.Execute(sql);
            int mentions = Convert.ToInt32(result[0]["Mentions"]!);
            if (mentions == 20)
                CreateAlert(targetId, DateTime.Now.AddDays(-30), DateTime.Now, "Target had 20+ mentions in the last 30 days");
                
            // Check for 3+ mentions in 15 min
            sql = $"SELECT COUNT(*) as Burst FROM Reports WHERE TargetId={targetId} AND SubmittedAt >= '{DateTime.Now.AddMinutes(-15):yyyy-MM-dd HH:mm:ss}'";
            result = DBConnection.Execute(sql);
            int burst = Convert.ToInt32(result[0]["Burst"]!);
            if (burst >= 3)
                CreateAlert(targetId, DateTime.Now.AddMinutes(-15), DateTime.Now, "Target had 3+ mentions in the last 15 minutes");
            
        }

        public static void CreateAlert(int targetId, DateTime windowStart, DateTime windowEnd, string reason)
        {
            string sql = $"INSERT INTO Alerts (TargetId, WindowStart, WindowEnd, Reason) VALUES ({targetId}, '{windowStart:yyyy-MM-dd HH:mm:ss}', '{windowEnd:yyyy-MM-dd HH:mm:ss}', '{reason.Replace("'", "''")}')";
            DBConnection.Execute(sql);
            Logger.Log($"AlertCreated: Target {targetId} - {reason}");
        }

        public static List<Dictionary<string, object?>> GetRecentAlerts()
        {
            string sql = "SELECT a.Id, p.FullName, a.WindowStart, a.WindowEnd, a.Reason FROM Alerts a JOIN People p ON a.TargetId = p.Id ORDER BY a.CreatedAt DESC LIMIT 10";
            return DBConnection.Execute(sql);
        }
    }
}