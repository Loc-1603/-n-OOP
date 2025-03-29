using System;
using System.Collections.Generic;
using System.Text;
using DoAnOOP.Models;
using DoAnOOP.Services;


namespace DoAnOOP
{
    class Program
    {
        static void Main()
        {

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Menu menu = new Menu();
            QuizManager quizManager = new QuizManager();
            quizManager.LoadQuizzes();
            SoundService soundService = SoundService.Instance;

            Console.Write("Nhập tên của bạn: ");
            string userName = Console.ReadLine();
            soundService.PlayEnterSound();

            User user = new User(userName);

            while (true)
            {
                int option = menu.ShowMainMenu();
                soundService.PlayEnterSound();

                if (option == 1) // Bắt đầu bài test
                {
                    List<Quiz> quizzes = quizManager.GetAllQuizzes();
                    if (quizzes.Count == 0)
                    {
                        Console.WriteLine("Chưa có bộ câu hỏi nào. Vui lòng thêm bộ câu hỏi trước.");
                        Console.ReadLine();
                        soundService.PlayEnterSound();
                    }
                    else
                    {
                        int quizIndex = menu.ChooseQuiz(quizzes);
                        Quiz selectedQuiz = quizzes[quizIndex];
                        TestSession session = new TestSession(user, selectedQuiz);
                        session.Start();
                    }
                }
                else if (option == 2) // Ôn tập
                {
                    List<Quiz> quizzes = quizManager.GetAllQuizzes();
                    if (quizzes.Count == 0)
                    {
                        Console.WriteLine("Chưa có bộ câu hỏi nào. Vui lòng thêm bộ câu hỏi trước.");
                        Console.ReadLine();
                        soundService.PlayEnterSound();
                        soundService.PlayEnterSound();
                    }
                    else
                    {
                        int quizIndex = menu.ChooseQuiz(quizzes);
                        Quiz selectedQuiz = quizzes[quizIndex];
                        TestSession session = new TestSession(user, selectedQuiz);
                        session.Review();
                    }
                }
                else if (option == 3) // Thêm bài test mới
                {
                    Console.Clear();
                    Quiz quiz = new Quiz();
                    Console.Write("Nhập tên bài test: ");
                    quiz.Title = Console.ReadLine();
                    soundService.PlayEnterSound();

                    Console.Write("Nhập số lượng câu hỏi: ");
                    int count = int.Parse(Console.ReadLine());
                    soundService.PlayEnterSound();
                    for (int i = 0; i < count; i++)
                    {
                        MultipleChoiceQuestion question = new MultipleChoiceQuestion();
                        Console.Write("Nhập câu hỏi: ");
                        question.Text = Console.ReadLine();
                        soundService.PlayEnterSound();

                        List<string> options = new List<string>();
                        for (int j = 0; j < 4; j++)
                        {
                            Console.Write("Nhập đáp án " + (j + 1) + ": ");
                            options.Add(Console.ReadLine());
                            soundService.PlayEnterSound();
                        }
                        question.Options = options;

                        Console.Write("Nhập đáp án đúng (1-4): ");
                        question.CorrectAnswer = Console.ReadLine();
                        soundService.PlayEnterSound();
                        quiz.Questions.Add(question);
                    }
                    quizManager.AddQuiz(quiz);
                    Console.WriteLine("Đã thêm bộ câu hỏi thành công!");
                    Console.WriteLine("Nhấn enter để quay lại menu chính");
                    Console.ReadLine();
                    soundService.PlayEnterSound();
                }
                else if (option == 4) // Xóa bài test
                {
                    Console.Clear();
                    List<Quiz> quizzes = quizManager.GetAllQuizzes();
                    if (quizzes.Count == 0)
                    {
                        Console.WriteLine("Hiện không có bộ câu hỏi nào để xóa.");
                        Console.ReadLine();
                    }
                    else
                    {
                        int quizIndex = menu.ChooseQuiz(quizzes);
                        quizManager.RemoveQuiz(quizIndex);
                        Console.WriteLine("Đã xóa bộ câu hỏi thành công!");
                        Console.WriteLine("Nhấn enter để quay lại menu chính");
                    }

                }
                else if (option == 5) // Xem quá trình hoạt động
                {
                    Logger.ViewLog();
                }
                else if (option == 6) // Thoát
                {
                    break;
                }
            }
        }
    }

}
