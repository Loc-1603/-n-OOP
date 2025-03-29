using System;
using System.Collections.Generic;

namespace DoAnOOP.Models
{
    // 2. Lớp câu hỏi trắc nghiệm
    [Serializable]
    public class MultipleChoiceQuestion : Question
    {
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }

        public MultipleChoiceQuestion()
        {
            Options = new List<string>();
        }

        public override void Display()
        {
            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + Options[i]);
            }
        }

        public override bool CheckAnswer(string answer)
        {
            return answer == CorrectAnswer;
        }
    }
}