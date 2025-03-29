using System.Collections.Generic;
using DoAnOOP.Models;

namespace DoAnOOP.Utils
{
    // 12. Lớp ScoreCalculator tính điểm dựa trên kết quả trả lời
    public static class ScoreCalculator
    {
        public static int CalculateScore(Quiz quiz, List<string> userAnswers)
        {
            int score = 0;
            for (int i = 0; i < quiz.Questions.Count; i++)
            {
                if (quiz.Questions[i].CheckAnswer(userAnswers[i]))
                {
                    score++;
                }
            }
            return score;
        }
    }
}