namespace Domain
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RoleDoorMapping> RoleDoorMappings { get; set; }
    }
}
