namespace Final_Task.Services
{
    public class FileSystemSaveLoadService : ISaveLoadService<string>
    {
        private readonly string _directoryPath;

        public FileSystemSaveLoadService(string directoryPath)
        {
            _directoryPath = directoryPath;
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        public void SaveData(string data, string identifier)
        {
            string filePath = Path.Combine(_directoryPath, identifier + ".txt");

            try
            {
                // Используем using для автоматического закрытия файлового потока
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(data);
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку или выбрасываем исключение с более информативным сообщением
                throw new InvalidOperationException($"Failed to save data to file {filePath}", ex);
            }
        }

        public string LoadData(string identifier)
        {
            string filePath = Path.Combine(_directoryPath, identifier + ".txt");

            try
            {
                if (File.Exists(filePath))
                {
                    // Используем using для автоматического закрытия файлового потока
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new FileNotFoundException($"File with identifier {identifier} not found.", filePath);
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку или выбрасываем исключение с более информативным сообщением
                throw new InvalidOperationException($"Failed to load data from file {filePath}", ex);
            }
        }
    }
}
