using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace SistemaOlcar.Helpers
{
    public class Encrypt
    {
        public static string EncriptarMD5(string clave)
        {
            string hash = "coding con c";
            byte[] data = UTF8Encoding.UTF8.GetBytes(clave);
            
            MD5 md5 = MD5.Create();
            TripleDES tripleDES = TripleDES.Create();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripleDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }

        //public static string Desencriptar(string clave)
        //{
        //    string hash = "coding con c";
        //    byte[] data = Convert.FromBase64String(clave);

        //    MD5 md5 = MD5.Create();
        //    TripleDES tripleDES = TripleDES.Create();

        //    tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
        //    tripleDES.Mode = CipherMode.ECB;

        //    ICryptoTransform transform = tripleDES.CreateDecryptor();
        //    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

        //    return UTF8Encoding.UTF8.GetString(result);
        //}

    }
}