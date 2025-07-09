using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace template_web_api.Common.Utils;

public class StringUtils
{
    /// <summary>
    /// StringFormatterUtils - จัดรูปแบบข้อความ
    /// </summary>
    public static class StringFormatterUtils
    {
        /// <summary>
        /// แปลงตัวอักษรแรกเป็นตัวพิมพ์ใหญ่
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการแปลง</param>
        /// <returns>ข้อความที่แปลงแล้ว</returns>
        public static string Capitalize(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input[1..].ToLower();
        }

        /// <summary>
        /// แปลงเป็น Title Case
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการแปลง</param>
        /// <returns>ข้อความ Title Case</returns>
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        /// <summary>
        /// แปลงเป็น camelCase
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการแปลง</param>
        /// <returns>ข้อความ camelCase</returns>
        public static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                    result.Append(words[i].ToLower());
                else
                    result.Append(Capitalize(words[i]));
            }

            return result.ToString();
        }

        /// <summary>
        /// แปลงเป็น PascalCase
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการแปลง</param>
        /// <returns>ข้อความ PascalCase</returns>
        public static string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();

            foreach (var word in words)
            {
                result.Append(Capitalize(word));
            }

            return result.ToString();
        }

        /// <summary>
        /// แปลงเป็น snake_case
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการแปลง</param>
        /// <returns>ข้อความ snake_case</returns>
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// แปลงเป็น kebab-case
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการแปลง</param>
        /// <returns>ข้อความ kebab-case</returns>
        public static string ToKebabCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1-$2").ToLower();
        }

        /// <summary>
        /// ตัดข้อความให้เหลือความยาวที่กำหนด
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการตัด</param>
        /// <param name="maxLength">ความยาวสูงสุด</param>
        /// <param name="suffix">ข้อความที่ต่อท้าย</param>
        /// <returns>ข้อความที่ตัดแล้ว</returns>
        public static string Truncate(string input, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;

            return input[..(maxLength - suffix.Length)] + suffix;
        }

        /// <summary>
        /// ลบ HTML tags
        /// </summary>
        /// <param name="input">ข้อความที่มี HTML tags</param>
        /// <returns>ข้อความที่ไม่มี HTML tags</returns>
        public static string RemoveHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        /// <summary>
        /// ลบอักขระพิเศษ
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการลบอักขระพิเศษ</param>
        /// <param name="replacement">ตัวแทนอักขระพิเศษ</param>
        /// <returns>ข้อความที่ไม่มีอักขระพิเศษ</returns>
        public static string RemoveSpecialCharacters(string input, string replacement = "")
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, @"[^a-zA-Z0-9\s]", replacement);
        }
    }

    /// <summary>
    /// StringValidationUtils - ตรวจสอบรูปแบบข้อความ
    /// </summary>
    public static class StringValidationUtils
    {
        /// <summary>
        /// ตรวจสอบว่าเป็นข้อความที่มีเฉพาะตัวเลข
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการตรวจสอบ</param>
        /// <returns>true หากเป็นตัวเลข</returns>
        public static bool IsNumeric(string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsDigit);
        }

        /// <summary>
        /// ตรวจสอบว่าเป็นข้อความที่มีเฉพาะตัวอักษร
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการตรวจสอบ</param>
        /// <returns>true หากเป็นตัวอักษร</returns>
        public static bool IsAlpha(string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsLetter);
        }

        /// <summary>
        /// ตรวจสอบว่าเป็นข้อความที่มีเฉพาะตัวอักษรและตัวเลข
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการตรวจสอบ</param>
        /// <returns>true หากเป็นตัวอักษรและตัวเลข</returns>
        public static bool IsAlphaNumeric(string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsLetterOrDigit);
        }

        /// <summary>
        /// ตรวจสอบว่าข้อความมีความยาวในช่วงที่กำหนด
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการตรวจสอบ</param>
        /// <param name="minLength">ความยาวขั้นต่ำ</param>
        /// <param name="maxLength">ความยาวสูงสุด</param>
        /// <returns>true หากความยาวอยู่ในช่วงที่กำหนด</returns>
        public static bool IsLengthInRange(string input, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return minLength == 0;

            return input.Length >= minLength && input.Length <= maxLength;
        }

        /// <summary>
        /// ตรวจสอบว่าข้อความตรงกับ Regular Expression
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการตรวจสอบ</param>
        /// <param name="pattern">รูปแบบ Regular Expression</param>
        /// <returns>true หากตรงกับรูปแบบ</returns>
        public static bool MatchesPattern(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
                return false;

            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// ตรวจสอบว่าเป็น URL ที่ถูกต้อง
        /// </summary>
        /// <param name="input">ข้อความที่ต้องการตรวจสอบ</param>
        /// <returns>true หากเป็น URL ที่ถูกต้อง</returns>
        public static bool IsValidUrl(string input)
        {
            return Uri.TryCreate(input, UriKind.Absolute, out var result)
                   && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
    }
}
