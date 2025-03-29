using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DoAnOOP.Models;

namespace DoAnOOP.Services
{
    // 4 Lớp LogEntry đại diện cho một bản ghi log trong hệ thống
    public static class Logger
    {
        private static string logFile = "quiz.json";

        public static void Log(string message)
        {
            List<LogEntry> logs = LoadLogs();
            logs.Add(new LogEntry(message));

            string jsonData = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFile, jsonData);
        }

        private static List<LogEntry> LoadLogs()
        {
            if (File.Exists(logFile))
            {
                try
                {
                    string jsonData = File.ReadAllText(logFile);
                    return JsonConvert.DeserializeObject<List<LogEntry>>(jsonData) ?? new List<LogEntry>();
                }
                catch (Exception)
                {
                    return new List<LogEntry>();
                }
            }
            return new List<LogEntry>();
        }

        public static void ViewLog()
        {
            if (File.Exists(logFile))
            {
                Console.Clear();
                Console.WriteLine("********** QUÁ TRÌNH HOẠT ĐỘNG **********");

                List<LogEntry> logEntries = LoadLogs();

                foreach (LogEntry logEntry in logEntries)
                {
                    Console.WriteLine(logEntry.ToString());
                }

                Console.WriteLine("Nhấn Enter để quay lại menu chính...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Không có dữ liệu log.");
                Console.ReadLine();
            }
        }
    }
}