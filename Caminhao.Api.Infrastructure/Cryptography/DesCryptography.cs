using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Caminhao.Api.Infrastructure.Cryptography
{
    public class DesCryptography : ICryptography {
        private readonly byte[] key = { 10, 20, 30, 40, 50, 60, 70, 80 };
        private readonly byte[] iv = { 10, 20, 30, 40, 50, 60, 70, 80 };
        private readonly Encoding encoding = Encoding.UTF8;
        
        public string Decrypt(string cipher) {
            try {
                DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
                byte[] inputByteArray = Convert.FromBase64String(cipher);

                MemoryStream Objmst = new MemoryStream();
                CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                Objcs.Write(inputByteArray, 0, inputByteArray.Length);
                Objcs.FlushFinalBlock();
                
                return encoding.GetString(Objmst.ToArray());
            } catch(Exception ex) {
                throw ex;
            }
        }

        public string Encrypt(string plain) {
            try {
                DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
                byte[] inputByteArray = encoding.GetBytes(plain);

                MemoryStream Objmst = new MemoryStream();
                CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                Objcs.Write(inputByteArray, 0, inputByteArray.Length);
                Objcs.FlushFinalBlock();

                return Convert.ToBase64String(Objmst.ToArray());
            } catch(Exception ex) {
                throw ex;
            }
        }
    }
}
