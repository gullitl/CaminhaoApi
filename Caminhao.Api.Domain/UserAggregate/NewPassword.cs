
namespace Caminhao.Api.Domain.UserAggregate
{
    public class NewPassword
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
