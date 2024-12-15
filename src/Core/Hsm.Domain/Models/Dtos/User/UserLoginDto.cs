namespace Hsm.Domain.Models.Dtos.User
{
    public class UserLoginDto
    {
        public string TcNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
