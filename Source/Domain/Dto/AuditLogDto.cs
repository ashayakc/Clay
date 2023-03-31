namespace Domain.Dto
{
    public class AuditLogDto
    {
        public long UserId { get; set; }
        public long DoorId { get; set; }
        public long OfficeId { get; set; }
        public byte Status { get; set; }
        public DateTime EventTime { get; set; }
        public string? Comments { get; set; }
    }
}
