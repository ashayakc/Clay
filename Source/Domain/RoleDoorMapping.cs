namespace Domain
{
    public class RoleDoorMapping
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long DoorId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Door Door { get; set; }
    }
}
