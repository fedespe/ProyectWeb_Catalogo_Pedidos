using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ET
{
    public static class Utilidades
    {
        public static string calculateMD5Hash(string input)

        {
            MD5 md5 = MD5.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] str = null;
            StringBuilder sb = new StringBuilder();
            str = md5.ComputeHash(encoding.GetBytes(input));
            for (int i = 0; i < str.Length; i++) sb.AppendFormat("{0:x2}", str[i]);

            string passwordMD5 = sb.ToString();

            return passwordMD5;
        }
    }
}
