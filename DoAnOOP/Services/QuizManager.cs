using System;
using System.Collections.Generic;
using DoAnOOP.Models;

namespace DoAnOOP.Services  
{
    // 7. Lớp QuizManager quản lý các bài test
    public class QuizManager  
    {
        private List<Quiz> quizzes;

        public QuizManager()
        {
            quizzes = new List<Quiz>();
            LoadQuizzes();
        }

        public void AddQuiz(Quiz quiz)
        {
            quizzes.Add(quiz);
            FileManager.SaveQuizzes(quizzes);
        }

        public void LoadQuizzes()
        {
            quizzes = FileManager.LoadQuizzes();
        }

        public List<Quiz> GetAllQuizzes()
        {
            return quizzes;
        }

        public void ClearQuizzes()
        {
            quizzes.Clear();
            FileManager.SaveQuizzes(quizzes);
        }

        public void RemoveQuiz(int index)
        {
            if (index >= 0 && index < quizzes.Count)
            {
                quizzes.RemoveAt(index);
                FileManager.SaveQuizzes(quizzes);
                Console.WriteLine("Đã xóa bài kiểm tra thành công!");
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ!");
            }
        }
    }
}