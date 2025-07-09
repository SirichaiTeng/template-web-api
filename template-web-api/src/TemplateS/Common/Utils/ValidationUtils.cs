using System.Text.RegularExpressions;

namespace template_web_api.Common.Utils;

public class ValidationUtils
{
    /// <summary>
    /// EmailValidationUtils - ตรวจสอบรูปแบบอีเมล
    /// </summary>
    public static class EmailValidationUtils
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// ตรวจสอบรูปแบบอีเมล
        /// </summary>
        /// <param name="email">อีเมลที่ต้องการตรวจสอบ</param>
        /// <returns>true หากรูปแบบอีเมลถูกต้อง</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// ตรวจสอบรูปแบบอีเมลหลายอัน
        /// </summary>
        /// <param name="emails">รายการอีเมลที่ต้องการตรวจสอบ</param>
        /// <param name="separator">ตัวคั่น</param>
        /// <returns>true หากอีเมลทั้งหมดถูกต้อง</returns>
        public static bool IsValidEmails(string emails, char separator = ',')
        {
            if (string.IsNullOrWhiteSpace(emails))
                return false;

            var emailList = emails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return emailList.All(email => IsValidEmail(email.Trim()));
        }

        /// <summary>
        /// แยกอีเมลที่ถูกต้องออกจากรายการ
        /// </summary>
        /// <param name="emails">รายการอีเมล</param>
        /// <param name="separator">ตัวคั่น</param>
        /// <returns>รายการอีเมลที่ถูกต้อง</returns>
        public static List<string> ExtractValidEmails(string emails, char separator = ',')
        {
            if (string.IsNullOrWhiteSpace(emails))
                return new List<string>();

            var emailList = emails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return emailList
                .Select(email => email.Trim())
                .Where(IsValidEmail)
                .ToList();
        }

        /// <summary>
        /// ตรวจสอบโดเมนอีเมล
        /// </summary>
        /// <param name="email">อีเมลที่ต้องการตรวจสอบ</param>
        /// <param name="allowedDomains">โดเมนที่อนุญาต</param>
        /// <returns>true หากโดเมนอนุญาต</returns>
        public static bool IsAllowedDomain(string email, string[] allowedDomains)
        {
            if (!IsValidEmail(email) || allowedDomains == null || allowedDomains.Length == 0)
                return false;

            var domain = email.Split('@')[1].ToLower();
            return allowedDomains.Any(d => d.ToLower() == domain);
        }
    }

    /// <summary>
    /// PhoneValidationUtils - ตรวจสอบเบอร์โทรศัพท์
    /// </summary>
    public static class PhoneValidationUtils
    {
        private static readonly Regex ThaiPhoneRegex = new Regex(
            @"^(\+66|0)(6|8|9)[0-9]{8}$",
            RegexOptions.Compiled);

        private static readonly Regex InternationalPhoneRegex = new Regex(
            @"^\+[1-9]\d{1,14}$",
            RegexOptions.Compiled);

        /// <summary>
        /// ตรวจสอบเบอร์โทรศัพท์ไทย
        /// </summary>
        /// <param name="phoneNumber">เบอร์โทรศัพท์</param>
        /// <returns>true หากเป็นเบอร์โทรศัพท์ไทยที่ถูกต้อง</returns>
        public static bool IsValidThaiPhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var cleanPhone = phoneNumber.Replace(" ", "").Replace("-", "");
            return ThaiPhoneRegex.IsMatch(cleanPhone);
        }

        /// <summary>
        /// ตรวจสอบเบอร์โทรศัพท์สากล
        /// </summary>
        /// <param name="phoneNumber">เบอร์โทรศัพท์</param>
        /// <returns>true หากเป็นเบอร์โทรศัพท์สากลที่ถูกต้อง</returns>
        public static bool IsValidInternationalPhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var cleanPhone = phoneNumber.Replace(" ", "").Replace("-", "");
            return InternationalPhoneRegex.IsMatch(cleanPhone);
        }

        /// <summary>
        /// จัดรูปแบบเบอร์โทรศัพท์ไทย
        /// </summary>
        /// <param name="phoneNumber">เบอร์โทรศัพท์</param>
        /// <returns>เบอร์โทรศัพท์ที่จัดรูปแบบแล้ว</returns>
        public static string FormatThaiPhone(string phoneNumber)
        {
            if (!IsValidThaiPhone(phoneNumber))
                return phoneNumber;

            var cleanPhone = phoneNumber.Replace(" ", "").Replace("-", "");

            // แปลง +66 เป็น 0
            if (cleanPhone.StartsWith("+66"))
                cleanPhone = "0" + cleanPhone[3..];

            // จัดรูปแบบเป็น 0XX-XXX-XXXX
            return $"{cleanPhone[..3]}-{cleanPhone[3..6]}-{cleanPhone[6..]}";
        }

        /// <summary>
        /// แปลงเบอร์โทรศัพท์ไทยเป็นรูปแบบสากล
        /// </summary>
        /// <param name="phoneNumber">เบอร์โทรศัพท์ไทย</param>
        /// <returns>เบอร์โทรศัพท์รูปแบบสากล</returns>
        public static string ToInternationalFormat(string phoneNumber)
        {
            if (!IsValidThaiPhone(phoneNumber))
                return phoneNumber;

            var cleanPhone = phoneNumber.Replace(" ", "").Replace("-", "");

            if (cleanPhone.StartsWith("0"))
                return "+66" + cleanPhone[1..];

            return cleanPhone;
        }
    }
}
