namespace JwtAuth.Dtos.Response
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
