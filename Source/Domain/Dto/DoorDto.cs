namespace Domain.Dto
{
    public class DoorDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public long OfficeId { get; set; }
    }
}
