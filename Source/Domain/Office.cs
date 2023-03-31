namespace Domain
{
    public class Office
    {
        public Office()
        {
            Doors = new HashSet<Door>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Door> Doors { get; set; }
    }
}
