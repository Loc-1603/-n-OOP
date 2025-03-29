using System;

namespace DoAnOOP.Models
{
    // 4 Lớp LogEntry đại diện cho một bản ghi log trong hệ thống
    [Serializable]
    public class LogEntry
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }

        public LogEntry(string message)
        {
            TimeStamp = DateTime.Now;
            Message = message;
        }

        public override string ToString()
        {
            return $"{TimeStamp:yyyy-MM-dd HH:mm:ss} - {Message}";
        }
    }
}