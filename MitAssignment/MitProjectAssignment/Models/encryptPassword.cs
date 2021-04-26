using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MitProjectAssignment.Models
{
    public static class encryptPassword
    {
        public static string textToEncrypt(string passWord)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(passWord)));
        }
    }
}