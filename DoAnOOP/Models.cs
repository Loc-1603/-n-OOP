
using System;
using System.Collections.Generic;

// 1. Lớp trừu tượng cho câu hỏi
[Serializable]
abstract class Question
{
    protected string text;

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public abstract void Display();
    public abstract bool CheckAnswer(string answer);
}

// 2. Lớp câu hỏi trắc nghiệm
[Serializable]
class MultipleChoiceQuestion : Question
{
    private List<string> options;
    private string correctAnswer;

    public List<string> Options
    {
        get { return options; }
        set { options = value; }
    }

    public string CorrectAnswer
    {
        get { return correctAnswer; }
        set { correctAnswer = value; }
    }

    public override void Display()
    {
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + options[i]);
        }
    }

    public override bool CheckAnswer(string answer)
    {
        return answer == correctAnswer;
    }
}

// 3. Lớp Quiz chứa danh sách câu hỏi
[Serializable]
class Quiz
{
    private string title;
    private List<Question> questions;

    public Quiz()
    {
        questions = new List<Question>();
    }

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public List<Question> Questions
    {
        get { return questions; }
        set { questions = value; }
    }
}

// 5. Lớp User đại diện cho người dùng làm bài test
class User
{
    private string name;

    public User(string nameInput)
    {
        name = nameInput;
    }

    public string Name
    {
        get { return name; }
    }
}