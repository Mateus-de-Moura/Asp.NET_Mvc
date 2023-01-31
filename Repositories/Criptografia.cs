using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebTeste.Repositories
{
    public class Criptografia
    {
        private string publickey = "12345678";
        private string secretkey = "87654321";

        public string Encript(string text)
        {
            try
            {              
                byte[] secretkeyByte = Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = Encoding.UTF8.GetBytes(publickey);

                byte[] inputbyteArray = Encoding.UTF8.GetBytes(text);
                using (var des = new DESCryptoServiceProvider())
                {
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    text = Convert.ToBase64String(ms.ToArray());
                }
                return text;
            }
            catch (Exception )
            {
                return "";
            }
        }
        public string Decript(string texto)
        {
            try
            {                
                byte[] privatekeyByte = Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = Encoding.UTF8.GetBytes(publickey);

                byte[] inputbyteArray = new byte[texto.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(texto.Replace(" ", "+"));
                using (var des = new DESCryptoServiceProvider())
                {
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    texto = encoding.GetString(ms.ToArray());
                }
                return texto;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }            
        }
    }

}

