using System.Security.Cryptography;
using System;

namespace CoWorking.App.Data.Tools
{
    public static class EncryptData{
        public static string EncrypText(string text){

            using (var sha256 = SHA256.Create()){
                var hasBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));

                var has = System.BitConverter.ToString(hasBytes).Replace("-","").ToLower();

                return has;
            }

        }

        
    }
}