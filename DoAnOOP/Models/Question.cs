using System;

namespace DoAnOOP.Models
{
    // 1. Lớp trừu tượng cho câu hỏi
    [Serializable]
    public abstract class Question
    {
        public string Text { get; set; }

        public abstract void Display();
        public abstract bool CheckAnswer(string answer);
    }
}