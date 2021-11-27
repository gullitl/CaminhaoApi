
namespace Caminhao.Api.Infrastructure.Cryptography
{
    public interface ICryptography {
        string Encrypt(string plain);
        string Decrypt(string cipher);
    }
}
