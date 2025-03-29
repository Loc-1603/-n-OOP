using System;
using System.Collections.Generic;

namespace DoAnOOP.Services
{
    // 14. Lớp ListShuffler xáo trộn các câu hỏi trong các bộ câu hỏi khi hiển thị
    public class ListShuffler
    {
        private Random random;

        public ListShuffler()
        {
            random = new Random();
        }

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
}