namespace Domain.Dto
{
    public class AuthenticateResponse
    {
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Username = user.UserName;
            Token = token;
        }
    }
}
