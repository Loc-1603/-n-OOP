using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;


namespace DoAnOOP
{
    // 1. Lớp trừu tượng cho câu hỏi
    [Serializable]
    abstract class Question
    {
        public string Text { get; set; }  

        public abstract void Display();
        public abstract bool CheckAnswer(string answer);
    }

    // 2. Lớp câu hỏi trắc nghiệm
    [Serializable]
    class MultipleChoiceQuestion : Question
    {
        // Đổi từ private field sang public property
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }

        // Constructor để khởi tạo Options
        public MultipleChoiceQuestion()
        {
            Options = new List<string>();
        }

        public override void Display()
        {
            // Giữ nguyên phần này
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

    // 3. Lớp Quiz chứa danh sách câu hỏi
    [Serializable]
    class Quiz
    {
        // Đổi từ private field sang public property
        public string Title { get; set; }
        public List<Question> Questions { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }
    }



    // 4 Lớp LogEntry đại diện cho một bản ghi log trong hệ thống
    [Serializable]  
    class LogEntry  // Tạo class mới để lưu thông tin log
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

    // 5. Lớp converter để xử lý serialization/deserialization của Question
    class QuestionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Question);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<MultipleChoiceQuestion>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    // 6. Lớp User đại diện cho người dùng làm bài test
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

    // 7. Lớp QuizManager quản lý các bài test
    class QuizManager
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

    // 8. Lớp TestSession quản lý một phiên làm bài test của người dùng
    class TestSession
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

    // 9. Lớp InputValidator xử lý ngoại lệ 
    public class InputValidator
    {
        public static int ValidateNumberInput(string input, int minValue, int maxValue)
        {
            if (!int.TryParse(input, out int number))
            {
                throw new ArgumentException("Vui lòng nhập số.");
            }

            if (number < minValue || number > maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    $"Vui lòng nhập số từ {minValue} đến {maxValue}.");
            }

            return number;
        }

        public static string GetValidAnswer()
        {
            string answer = "";
            bool isValid = false;

            while (!isValid)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter && answer.Length > 0)
                {
                    isValid = true;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    throw new OperationCanceledException("Đã hủy làm bài.");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && answer.Length > 0)
                {
                    answer = answer.Substring(0, answer.Length - 1);
                    Console.Write("\b \b");
                }
                else if (char.IsDigit(keyInfo.KeyChar))
                {
                    int digit = int.Parse(keyInfo.KeyChar.ToString());
                    if (digit >= 1 && digit <= 4)
                    {
                        answer = digit.ToString();
                        Console.Write(keyInfo.KeyChar);
                    }
                }
            }

            return answer;
        }
    }

    // 10. Lớp FileManager xử lý thao tác đọc và ghi file
    static class FileManager
    {
        private const string filePath = "BoCauHoi.json";

        public static void SaveQuizzes(List<Quiz> quizzes)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            List<JsonConverter> converters = new List<JsonConverter>();
            converters.Add(new QuestionConverter());
            settings.Converters = converters;

            string jsonData = JsonConvert.SerializeObject(quizzes, Formatting.Indented, settings);
            File.WriteAllText(filePath, jsonData);
        }

        public static List<Quiz> LoadQuizzes()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Converters = new List<JsonConverter> { new QuestionConverter() }
                };

                return JsonConvert.DeserializeObject<List<Quiz>>(jsonData, settings);
            }
            return new List<Quiz>();
        }
    }

    // 11. Lớp Menu hiển thị giao diện cho người dùng
    class Menu
    {
        private readonly SoundService _soundService;

        public Menu()
        {
            _soundService = SoundService.Instance;
        }
        public int ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╔══════════════════════════╗");
                Console.WriteLine("║        MENU CHÍNH        ║");
                Console.WriteLine("╚══════════════════════════╝");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. Bắt đầu bài test (5 câu hỏi ngẫu nhiên)");
                Console.WriteLine("2. Ôn tập (tất cả câu hỏi)");
                Console.WriteLine("3. Thêm bài test mới");
                Console.WriteLine("4. Xóa bài test");
                Console.WriteLine("5. Xem quá trình hoạt động");
                Console.WriteLine("6. Thoát");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("👉 Chọn: ");
                Console.ResetColor();

                string input = Console.ReadLine();
                _soundService.PlayEnterSound();

                if (int.TryParse(input, out int option) && option >= 1 && option <= 6)
                {
                    return option;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập số từ 1 đến 6.");
                Console.ResetColor();
                Console.WriteLine("Nhấn Enter để tiếp tục...");
                Console.ReadLine();
            }
        }

        public int ChooseQuiz(List<Quiz> quizzes)
        {
            SoundService soundService = SoundService.Instance;
            while (true) 
            {
                Console.Clear();
                Console.WriteLine("Chọn bài test:");
                for (int i = 0; i < quizzes.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + quizzes[i].Title);
                }
                Console.Write("Chọn: ");

                string input = Console.ReadLine();
                soundService.PlayEnterSound();

                if (int.TryParse(input, out int choice))
                {
                    // Kiểm tra xem số nhập có nằm trong phạm vi danh sách bài test không
                    if (choice >= 1 && choice <= quizzes.Count)
                    {
                        return choice - 1; // Trả về chỉ số hợp lệ
                    }
                    else
                    {
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                    }
                }
                else
                {
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập số.");
                }

                Console.WriteLine("Nhấn Enter để tiếp tục...");
                Console.ReadLine();
            }
        }
    }


    // 12. Lớp ScoreCalculator tính điểm dựa trên kết quả trả lời
    static class ScoreCalculator
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

    // 13. Lớp Logger ghi lại các hoạt động của chương trình
    static class Logger
    {
        private static string logFile = "quiz.json";

        public static void Log(string message)
        {
            List<LogEntry> logs = LoadLogs();  // Đọc logs cũ
            logs.Add(new LogEntry(message));   // Thêm log mới

            // Lưu toàn bộ logs
            string jsonData = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(logFile, jsonData);
        }

        private static List<LogEntry> LoadLogs()
        {
            if (File.Exists(logFile))
            {
                try
                {
                    string jsonData = File.ReadAllText(logFile);
                    return JsonConvert.DeserializeObject<List<LogEntry>>(jsonData) ?? new List<LogEntry>();
                }
                catch (Exception)
                {
                    return new List<LogEntry>();
                }
            }
            return new List<LogEntry>();
        }

        public static void ViewLog()
        {
            if (File.Exists(logFile))
            {
                Console.Clear();
                Console.WriteLine("********** QUÁ TRÌNH HOẠT ĐỘNG **********");

                List<LogEntry> logEntries = LoadLogs();

                foreach (LogEntry logEntry in logEntries)
                {
                    Console.WriteLine(logEntry.ToString());
                }

                Console.WriteLine("Nhấn Enter để quay lại menu chính...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Không có dữ liệu log.");
                Console.ReadLine();
            }
        }
    }

    // 14. Lớp ListShuffler xáo trộn các câu hỏi trong các bộ câu hỏi khi hiển thị
    class ListShuffler
    {
        private Random random;

        public ListShuffler()
        {
            random = new Random();
        }

        // Phương thức xáo trộn danh sách
        public void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    // 15. Lớp SoundService phát âm thanh
    public sealed class SoundService : IDisposable
    {
        private static readonly Lazy<SoundService> _instance = new Lazy<SoundService>(() => new SoundService());
        private SoundPlayer _player;
        private readonly string _soundFilePath;

        // Singleton pattern để đảm bảo chỉ có 1 instance
        public static SoundService Instance => _instance.Value;

        private SoundService()
        {
            // Khởi tạo đường dẫn file âm thanh
            _soundFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pick.wav");

            // Khởi tạo SoundPlayer
            _player = new SoundPlayer();

            // Tải âm thanh vào bộ nhớ
            LoadSound();
        }

        private void LoadSound()
        {
            try
            {
                if (File.Exists(_soundFilePath))
                {
                    _player.SoundLocation = _soundFilePath;
                    _player.LoadAsync(); // Tải âm thanh bất đồng bộ
                }
                else
                {
                    Console.WriteLine($"Không tìm thấy file âm thanh: {_soundFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải âm thanh: {ex.Message}");
            }
        }

        public void PlayEnterSound()
        {
            if (_player.IsLoadCompleted)
            {
                try
                {
                    _player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi phát âm thanh: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            _player?.Dispose();
        }
    }

    // Lớp program
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
