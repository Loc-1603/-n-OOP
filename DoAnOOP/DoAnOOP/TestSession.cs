using System;
using System.Collections.Generic;

public class TestSession
{
    private readonly Quiz quiz;
    private readonly SoundService _soundService;
    private int score;

    public TestSession(Quiz quiz, SoundService soundService)
    {
        this.quiz = quiz;
        _soundService = soundService;
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
        int questionsToTake = Math.Min(5, quiz.Questions.Count);
        for (int i = 0; i < questionsToTake; i++)
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
    }
} 