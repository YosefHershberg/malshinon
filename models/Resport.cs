
namespace Models
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string? ReportText { get; set; }
        public DateTime SubmittedAt { get; set; }

        public Report(int id, int reporterId, int targetId, string? reportText, DateTime submittedAt)
        {
            Id = id;
            ReporterId = reporterId;
            TargetId = targetId;
            ReportText = reportText;
            SubmittedAt = submittedAt;
        }
    }
}