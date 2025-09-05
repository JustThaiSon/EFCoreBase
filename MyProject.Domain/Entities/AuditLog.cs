namespace MyProject.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TableName { get; set; } = default!;
        public string? KeyValues { get; set; } 
        public string? OldValues { get; set; } 
        public string? NewValues { get; set; } 
        public Guid? UserId { get; set; }
        public string ActionType { get; set; } = default!;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
    }

}
