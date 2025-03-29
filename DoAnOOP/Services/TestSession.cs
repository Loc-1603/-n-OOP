using System;
using System.Collections.Generic;
using DoAnOOP.Models;
using DoAnOOP.Utils;

namespace DoAnOOP.Services  
{
    // 8. Lớp TestSession quản lý một phiên làm bài test và ôn tập của người dùng
    public class TestSession  
    {
        private User user;
        private Quiz quiz;
        private int score;
        private readonly SoundService _soundService;

        public TestSession(User testUser, Quiz testQuiz)
        {
            user = testUser;
            quiz = testQuiz;
            score = 0;
            _soundService = SoundService.Instance;
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Bắt đầu bài test: " + quiz.Title);
            Console.WriteLine("Nhấn Enter để tiếp tục...");
            Console.ReadLine();
            _soundService.PlayEnterSound();

            ListShuffler shuffler = new ListShuffler();
            shuffler.Shuffle(quiz.Questions);

            List<Question> selectedQuestions = new List<Question>();
            int questionCount = quiz.Questions.Count < 5 ? quiz.Questions.Count : 5;
            for (int i = 0; i < questionCount; i++)
            {
                selectedQuestions.Add(quiz.Questions[i]);
            }

            for (int i = 0; i < selectedQuestions.Count; i++)
            {
                Question question = selectedQuestions[i];
                Console.Clear();
                Console.WriteLine("Câu hỏi " + (i + 1) + ": " + question.Text);
                question.Display();

                Console.Write("Trả lời (1-4): ");
                try
                {
                    string answer = InputValidator.GetValidAnswer();
                    _soundService.PlayEnterSound();

                    if (question.CheckAnswer(answer))
                    {
                        score++;
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("\nĐã quay lại menu chính.");
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine($"{user.Name}, bạn đã trả lời đúng {score} trên 5 câu hỏi.");
            Logger.Log($"{user.Name} làm bài test \"{quiz.Title}\" đạt {score}/5");

            Console.WriteLine("Nhấn Enter để quay lại menu chính...");
            Console.ReadLine();
            _soundService.PlayEnterSound();
        }

        public void Review()
        {
            Console.Clear();
            Console.WriteLine("Ôn tập: " + quiz.Title);
            Console.WriteLine("Nhấn Enter để tiếp tục...");
            Console.ReadLine();
            _soundService.PlayEnterSound();

            for (int i = 0; i < quiz.Questions.Count; i++)
            {
                Question question = quiz.Questions[i];
                Console.Clear();
                Console.WriteLine("Câu hỏi " + (i + 1) + ": " + question.Text);
                question.Display();

                Console.Write("Trả lời (1-4): ");
                try
                {
                    string answer = InputValidator.GetValidAnswer();
                    _soundService.PlayEnterSound();

                    if (question.CheckAnswer(answer))
                    {
                        Console.WriteLine("\nChính xác!");
                    }
                    else
                    {
                        Console.WriteLine("\nSai rồi!");
                        Console.WriteLine("Đáp án đúng phải là: " +
                            ((MultipleChoiceQuestion)question).CorrectAnswer);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("\nĐã quay lại menu chính.");
                    return;
                }

                Console.WriteLine("Nhấn Enter để tiếp tục...");
                Console.ReadLine();
                _soundService.PlayEnterSound();
            }

            Console.Clear();
            Console.WriteLine("Bạn đã hoàn thành ôn tập.");
            Console.WriteLine("Nhấn Enter để quay lại menu chính...");
            Console.ReadLine();
            _soundService.PlayEnterSound();
        }
    }
}