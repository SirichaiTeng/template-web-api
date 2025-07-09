namespace template_web_api.Common.Utils;

public class FileUtils
{
    public static class FileUploadUtils
    {
        /// <summary>
        /// ตรวจสอบประเภทไฟล์ที่อนุญาต
        /// </summary>
        /// <param name="file">ไฟล์ที่ต้องการตรวจสอบ</param>
        /// <param name="allowedTypes">ประเภทไฟล์ที่อนุญาต</param>
        /// <returns>true หากเป็นประเภทไฟล์ที่อนุญาต</returns>
        public static bool ValidateFileType(IFormFile file, string[] allowedTypes = null)
        {
            allowedTypes ??= new[] { "image/jpeg", "image/png", "application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            return allowedTypes.Contains(file.ContentType);
        }

        /// <summary>
        /// ตรวจสอบขนาดไฟล์
        /// </summary>
        /// <param name="file">ไฟล์ที่ต้องการตรวจสอบ</param>
        /// <param name="maxSizeInMB">ขนาดสูงสุดที่อนุญาต (MB)</param>
        /// <returns>true หากขนาดไฟล์ไม่เกินที่กำหนด</returns>
        public static bool ValidateFileSize(IFormFile file, int maxSizeInMB = 5)
        {
            var maxSizeInBytes = maxSizeInMB * 1024 * 1024;
            return file.Length <= maxSizeInBytes;
        }

        /// <summary>
        /// แปลงไฟล์เป็น Base64
        /// </summary>
        /// <param name="file">ไฟล์ที่ต้องการแปลง</param>
        /// <returns>Base64 string</returns>
        public static async Task<string> FileToBase64Async(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// สร้างชื่อไฟล์ที่ไม่ซ้ำกัน
        /// </summary>
        /// <param name="originalFileName">ชื่อไฟล์เดิม</param>
        /// <returns>ชื่อไฟล์ใหม่</returns>
        public static string GenerateUniqueFileName(string originalFileName)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var guid = Guid.NewGuid().ToString("N")[..8];
            var extension = Path.GetExtension(originalFileName);
            return $"{timestamp}_{guid}{extension}";
        }

        /// <summary>
        /// บันทึกไฟล์ลงเซิร์ฟเวอร์
        /// </summary>
        /// <param name="file">ไฟล์ที่ต้องการบันทึก</param>
        /// <param name="path">เส้นทางที่ต้องการบันทึก</param>
        /// <returns>เส้นทางไฟล์ที่บันทึก</returns>
        public static async Task<string> SaveFileAsync(IFormFile file, string path)
        {
            var fileName = GenerateUniqueFileName(file.FileName);
            var filePath = Path.Combine(path, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath;
        }
    }
}
