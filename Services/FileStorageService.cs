using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PharmacyWarehouse.Services;

// Универсальный класс для сохранения и загрузки данных в JSON-файлы
public class FileStorageService<T>
{
    private string filePath; // Путь к файлу

    public FileStorageService(string filePath)
    {
        this.filePath = filePath;
    }
    
    // Сохранение списка объектов в файл
    public void Save(List<T> data)
    {
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }
    
    // Загрузка списка объектов из файла
    public List<T> Load()
    {
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }
}