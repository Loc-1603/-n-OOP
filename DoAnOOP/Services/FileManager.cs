using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DoAnOOP.Models;
using DoAnOOP.Utils;

namespace DoAnOOP.Services
{
    // 10. Lớp FileManager xử lý thao tác đọc và ghi file
    public static class FileManager
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
}