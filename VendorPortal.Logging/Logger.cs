using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace VendorPortal.Logging
{
    public static class Logger
    {


        /// <summary>
        /// ใช้สำหรับเขียน Log ของการเรียกใช้งาน API ที่เกิด Exception เท่านั้น
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="name">ให้ใชช้ชื่อของ Function มีการเรียกเข้ามา</param>
        /// <param name="request">request คือ Parametor ที่ส่งเข้ามาทำงานที่ Function นี้ แต่ถ้าไม่มีก็ไม่จำเป็นต้องส่งเข้ามา</param>
        public async static void LogError(Exception ex, string name, string? request = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            string _LogFile = configuration["Logging:Path:Directory"] ?? "";
            if (string.IsNullOrEmpty(_LogFile))
            {
                throw new InvalidOperationException("LogFile path is not configured.");
            }
            string guid = Guid.NewGuid().ToString();
            string basePath = $"{_LogFile}/{DateTime.Now.Date:yyyyMMdd}/{name}";
            try
            {
                if (!Directory.Exists($"{basePath}"))
                    Directory.CreateDirectory(basePath);
                using (StreamWriter sw = new StreamWriter($"{basePath}/ErrorLog.txt", true))
                {
                    await sw.WriteLineAsync($"----------------------- Start of {guid} ----------------------------");
                    await sw.WriteLineAsync($"Date of Error : {DateTime.Now}");
                    await sw.WriteLineAsync($"{ex.Message}");
                    await sw.WriteLineAsync($"{ex.StackTrace}");
                    await sw.WriteLineAsync($"{ex.InnerException}");
                    if (!string.IsNullOrEmpty(request))
                    {
                        await sw.WriteLineAsync($"{request}");
                    }
                    await sw.WriteLineAsync($"----------------------- End of {guid} ------------------------------");
                }

            }
            catch{ }

        }
        public async static void LogInfo(string message, string name, string? request = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            string _LogFile = configuration["Logging:Path:Directory"] ?? "";
            if (string.IsNullOrEmpty(_LogFile))
            {
                throw new InvalidOperationException("LogFile path is not configured.");
            }
            string basePath = $"{_LogFile}/{DateTime.Now.Date:yyyyMMdd}";
            try
            {
                if (!Directory.Exists($"{basePath}"))
                    Directory.CreateDirectory(basePath);
                using (StreamWriter sw = new StreamWriter($"{basePath}/{name}_InfoLog.txt", true))
                {
                    await sw.WriteLineAsync($"{message}");
                    await sw.WriteLineAsync($"");
                    await sw.WriteLineAsync($"Date of Info : {DateTime.Now}");
                    if (!string.IsNullOrEmpty(request))
                    {
                        await sw.WriteLineAsync($"{request}");
                    }
                }

            }
            catch { }

        }
    }
}