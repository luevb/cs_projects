using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;
using LogAnalyzer.Models;

namespace LogAnalyzer
{
    class Program
    {
        const string INPUT_LOG_FILE = "events.log";
        const string OUTPUT_CSV_FILE = "analysis.csv";
        const int targetUserId = 2;
        static int processedLinesForUser = 0;
        static int totalLines = 0;
        static int parseErrors = 0;
        static int loginCount = 0;
        static int logoutCount = 0;
        static int errorCount = 0;
        static int purchaseCount = 0;
        static DateTime? firstEventTimestamp = null;
        static DateTime? lastEventTimestamp = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Консольное приложение, JSON парсинга. ББСО-02-24 Артыков Самандр. Вариант 2");
            Console.WriteLine();

            Dictionary<string, int> eventStats = new Dictionary<string, int>();
            const int bufferSize = 65536;
            try
            {
                using (var fileStream = new FileStream(INPUT_LOG_FILE, FileMode.Open))
                using (var reader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
                {
                    string? line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        totalLines++;

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        try
                        {
                            ProcessLine(line, eventStats);
                        }
                        catch (JsonException ex)
                        {
                            parseErrors++;
                            Console.WriteLine($"Ошибка в строке {totalLines}: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            parseErrors++;
                            Console.WriteLine($"Ошибка в строке {totalLines}: {ex.Message}");
                        }

                        if (totalLines % 1000 == 0)
                        {
                            Console.WriteLine($"Прогресс: {totalLines} строк, ошибок: {parseErrors}");
                        }
                    }
                }

                SaveResultToCsv(eventStats, OUTPUT_CSV_FILE);

                Console.WriteLine($"Дата самого раннего события для ID {targetUserId}: {firstEventTimestamp}");
                Console.WriteLine($"Дата самого позднего события для ID {targetUserId}: {lastEventTimestamp}");
                Console.WriteLine();
                Console.WriteLine($"Статистика по событиям:");
                Console.WriteLine($"  login: {loginCount}");
                Console.WriteLine($"  logout: {logoutCount}");
                Console.WriteLine($"  error: {errorCount}");
                Console.WriteLine($"  purchase: {purchaseCount}");
                Console.WriteLine();
                Console.WriteLine($"Всего обработано строк: {totalLines}");
                Console.WriteLine($"Ошибок при парсинге: {parseErrors}");
                Console.WriteLine($"Строк для пользователя {targetUserId}: {processedLinesForUser}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл '{INPUT_LOG_FILE}' не найден!");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Готово! Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ProcessLine(string line, Dictionary<string, int> eventStats)
        {
            int separatorIndex = line.IndexOf(" - ");
            if (separatorIndex == -1)
            {
                parseErrors++;
                Console.WriteLine($"Строка {totalLines} содержит ошибку.");
                return;
            }

            string dateStr = line.Substring(0, separatorIndex);
            string jsonStr = line.Substring(separatorIndex + 3);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            LogEvent? logEvent = JsonSerializer.Deserialize<LogEvent>(jsonStr, options);

            if (logEvent == null)
            {
                Console.WriteLine($"Не удалось десериализовать JSON в строке {totalLines}");
                return;
            }

            if (logEvent.UserId == targetUserId && DateTime.TryParse(dateStr, out DateTime eventDate))
            {
                processedLinesForUser++;

                if (logEvent.EventType == "login") { loginCount++; }
                if (logEvent.EventType == "logout") { logoutCount++; }
                if (logEvent.EventType == "error") { errorCount++; }
                if (logEvent.EventType == "purchase") { purchaseCount++; }

                if (firstEventTimestamp == null || eventDate < firstEventTimestamp)
                    firstEventTimestamp = eventDate;

                if (lastEventTimestamp == null || eventDate > lastEventTimestamp)
                    lastEventTimestamp = eventDate;

                string statsKey = $"{logEvent.Ip};{logEvent.EventType}";

                if (eventStats.ContainsKey(statsKey))
                    eventStats[statsKey]++;
                else
                    eventStats[statsKey] = 1;
            }
        }

        static void SaveResultToCsv(Dictionary<string, int> stats, string filename)
        {
            using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                writer.WriteLine("IP;EventType;Count");

                if (stats.Count > 0)
                {
                    foreach (var entry in stats.OrderBy(k => k.Key))
                    {
                        writer.WriteLine($"{entry.Key};{entry.Value}");
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Результаты сохранены в '{filename}'");
                }
                else
                {
                    writer.WriteLine($"Нет данных пользователя с ID = {targetUserId}");
                    Console.WriteLine($"Для пользователя {targetUserId} ничего не найдено.");
                }
            }
        }
    }
}