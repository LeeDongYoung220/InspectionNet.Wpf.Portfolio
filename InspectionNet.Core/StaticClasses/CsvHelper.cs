using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionNet.Core.StaticClasses
{
    public static class CsvHelper
    {
        public static void SaveToCsv(Dictionary<string, object> data, string filePath)
        {
            var sb = new StringBuilder();

            // 1. 헤더 작성
            sb.AppendLine(string.Join(",", data.Keys));

            // 2. 값 작성 (object → string 변환)
            List<string> values = [];
            foreach (var value in data.Values)
            {
                if (value == null)
                    values.Add(""); // null 처리
                else
                    values.Add(EscapeForCsv(value.ToString()));
            }

            sb.AppendLine(string.Join(",", values));

            // 3. 파일 저장
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public static void SaveToCsv(List<Dictionary<string, object>> dataList, string filePath)
        {
            if (dataList == null || dataList.Count == 0)
                throw new ArgumentException("데이터가 비어 있습니다.");

            var allKeys = dataList.SelectMany(dict => dict.Keys).Distinct().ToList(); // 모든 키 수집
            var sb = new StringBuilder();

            // 1. 헤더 작성
            sb.AppendLine(string.Join(",", allKeys));

            // 2. 데이터 행 작성
            foreach (var dict in dataList)
            {
                var row = new List<string>();
                foreach (var key in allKeys)
                {
                    dict.TryGetValue(key, out object? value);
                    row.Add(EscapeForCsv(value?.ToString() ?? ""));
                }
                sb.AppendLine(string.Join(",", row));
            }

            // 3. 파일 저장
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public static void AppendToCsv(Dictionary<string, object> data, string filePath)
        {
            bool fileExists = File.Exists(filePath);
            var sb = new StringBuilder();

            // 파일이 없을 경우, 헤더 추가
            if (!fileExists)
            {
                sb.AppendLine(string.Join(",", data.Keys));
            }

            // 값 작성
            List<string> values = [];
            foreach (var value in data.Values)
            {
                if (value == null)
                    values.Add("");
                else
                    values.Add(EscapeForCsv(value.ToString()));
            }
            sb.AppendLine(string.Join(",", values));

            // 파일에 이어쓰기 (Append)
            File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string EscapeForCsv(string? input)
        {
            if (input == null) return string.Empty;
            // 쉼표, 큰따옴표, 줄바꿈 처리
            if (input.Contains(',') || input.Contains('"') || input.Contains('\n'))
            {
                input = input.Replace("\"", "\"\"");
                return $"\"{input}\"";
            }
            else
            {
                return input;
            }
        }
    }
}
