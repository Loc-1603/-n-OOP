using System;
using System.Collections.Generic;

namespace DoAnOOP.Models
{
    // 3. Lớp Quiz chứa danh sách câu hỏi
    [Serializable]
    public class Quiz
    {
        public string Title { get; set; }
        public List<Question> Questions { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }
    }
}