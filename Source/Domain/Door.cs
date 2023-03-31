namespace Domain
{
    public class Door
    {
        public Door()
        {
            RoleDoorMappings = new HashSet<RoleDoorMapping>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long OfficeId { get; set; }

        public virtual Office Office { get; set; }
        public virtual ICollection<RoleDoorMapping> RoleDoorMappings { get; set; }
    }
}
