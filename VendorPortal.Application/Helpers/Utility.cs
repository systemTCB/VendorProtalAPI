using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using VendorPortal.Logging;

namespace VendorPortal.Application.Helpers
{
    public static class Utility
    {
        //Function Encrypt And Decrypt
        private readonly static string key = "!wolfXkubboss@2025";
        private readonly static string salt = "TechconsBiz@2024";

        public static string EncyptionToken(string token, DateTime? duration)
        {
            var enctptedToken = string.Empty;
            try
            {
                if(string.IsNullOrEmpty(token) || duration == null) return "InvalidToken";
                enctptedToken = EncryptString(token + "|Expire:" + duration);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "EncyptionToken");
                return "EncyptionFailed";
            }
            return enctptedToken;
        }
        public static string DecyptionToken(string token)
        {
            var decyptedToken = string.Empty;
            try
            {
                if(string.IsNullOrEmpty(token)) return "InvalidToken";
                decyptedToken = DecryptString(token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "DecyptionToken");
                return "DecyptionFailed";
            }
            return decyptedToken;
        }
        public static string EncryptString(string encryptString)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptString)) return encryptString;

                byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString); //แปลง key เป็น byte เพื่อใช้ในการสร้างรหัส
                using (Aes encryptor = Aes.Create()) //เปิดฟังชั่น Aes เพื่อทำการสร้าง Encrypt data
                {
                    //การเข้ารหัส key ในการสร้าง Encrypt data 
                    Rfc2898DeriveBytes pdb = new(key, Encoding.UTF8.GetBytes(salt), 10000, HashAlgorithmName.SHA256);
                    encryptor.Key = pdb.GetBytes(32); //นำ key มาเข้ารหัสข้อมูลทีละ block 
                    encryptor.IV = pdb.GetBytes(16); //ทำการกำหนดขนาด data initialization vector
                    using (MemoryStream ms = new())
                    {
                        using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        encryptString = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return encryptString;
            }
            catch
            {
                return encryptString;
            }
        }

        public static string DecryptString(string cipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText)) return cipherText;

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    //การเข้ารหัส key ในการสร้าง Decrypt data โดย key ที่ใช้ต้องเหมือนกับ key ที่ใช้ในการสร้าง Encrypt data
                    Rfc2898DeriveBytes pdb = new(key, Encoding.UTF8.GetBytes(salt), 10000, HashAlgorithmName.SHA256);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {
                return cipherText;
            }

        }
    
        public static List<T> PageCalculator<T>(List<T> list, int page, int pageSize)
        {
            try
            {
                if (list == null || list.Count == 0) return list;
                if (page <= 0 || pageSize <= 0) return list;
                int skip = (page - 1) * pageSize;
                int take = pageSize;
                return list.GetRange(skip, take);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PageCalculator");
                return list;
            }
        }
    }
}