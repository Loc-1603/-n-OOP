using System;

namespace DoAnOOP.Utils
{
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
}