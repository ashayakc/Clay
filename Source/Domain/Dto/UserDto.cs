namespace Domain.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public string EmailId { get; set; } = null!;
    }
}
