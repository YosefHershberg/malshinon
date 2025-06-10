
namespace Models
{
    public class Alert
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public DateTime WindowStart { get; set; }
        public DateTime WindowEnd { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }

        public Alert(int id, int targetId, DateTime windowStart, DateTime windowEnd, string? reason, DateTime createdAt)
        {
            Id = id;
            TargetId = targetId;
            WindowStart = windowStart;
            WindowEnd = windowEnd;
            Reason = reason;
            CreatedAt = createdAt;
        }
    }
}