namespace Domain
{
    public class User
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string EmployeeId { get; set; }
        public string EmailId { get; set; }
        public byte IsAdmin { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long RoleId { get; set; }

        public virtual Role Roles { get; set; }
    }
}
