// 11. Lớp Menu hiển thị giao diện cho người dùng
using DoAnOOP;
using System.Collections.Generic;
using System;
using DoAnOOP.Services;
using DoAnOOP.Models;

public class Menu
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