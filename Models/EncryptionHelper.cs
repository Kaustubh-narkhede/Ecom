using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_Project_dotnetcore.Models
{
    public class EncryptionHelper : Controller
    {
        public string EncryptString(string json)
        {

            // data objData=getJsonstring(json);
            string plainText = "", key = "1234567891234567";
            plainText = getJsonstring(json);
            byte[] iv = new byte[16];
            byte[] array;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }
                            array = memoryStream.ToArray();
                        }
                    }
                }
                return Convert.ToBase64String(array);
            }
            catch (Exception ex) { return ex.Message.ToString(); }


        }

        public string DecryptString(string cipherText)
        {
            var statuscode = 1;
            try
            {
                byte[] iv = new byte[16];
                string key = "1234567891234567";
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { statuscode = 0; return statuscode.ToString(); }
        }


        public string getJsonstring(string json)
        {
            data objData = new data();
            string plainText = "";
            if (!string.IsNullOrEmpty(json))
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                foreach (var kv in dict)
                {
                    if (kv.Key == "type") objData.type = kv.Value;
                    if (kv.Key == "userId") objData.userId = kv.Value;
                    if (kv.Key == "expiry") objData.expiry = kv.Value;
                }

                plainText = "{\"type\":\"" + objData.type + "\",\"userId\":\"" + objData.userId + "\",\"expiry\":\"" + objData.expiry + "\"}";
            }
            return plainText;
        }
    }

    public class data
    {
        public string type { get; set; }
        public string userId { get; set; }
        public string expiry { get; set; }
    }

}
