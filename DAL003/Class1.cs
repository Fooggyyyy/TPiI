using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Text;
using Newtonsoft.Json;

namespace DAL003
{
    public record Celebrity(int id, string Firstname, string Surname, string PhotoPath);
    public interface IRepository : IDisposable  
    {
        string BasePath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebrityByid(int id);
        Celebrity[] GetCelebrityBySurname(string Surname);
        string? getPhotoPathByid(int id);
    }
  
    public class Repository : IRepository
    {
        public static string JSONFileName { get; } = "Сelebrities.json";
        public string BasePath { get; }
        private List<Celebrity> AllCelebrity { get; set; }

        private Repository(string directoryPath)
        {
            BasePath = "C:\\Users\\user\\source\\repos\\ASPA\\DAL003\\" + directoryPath + "\\Сelebrities.json";

            
            if (File.Exists(BasePath))
            {
                var jsonData = File.ReadAllText(BasePath);
                AllCelebrity = JsonConvert.DeserializeObject<List<Celebrity>>(jsonData) ?? new List<Celebrity>();
            }
            else
            {
                AllCelebrity = new List<Celebrity>();
            }

        }

        public static Repository Create(string directoryName)
        {
            return new Repository(directoryName);
        }

        public Celebrity[] getAllCelebrities()
        {
            return AllCelebrity.ToArray();
        }

        public Celebrity? getCelebrityByid(int id)
        {
            return AllCelebrity.FirstOrDefault(x => x.id == id);
        }

        public Celebrity[] GetCelebrityBySurname(string surname)
        {
            return AllCelebrity.Where(x => x.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public string? getPhotoPathByid(int id)
        {
            return AllCelebrity.FirstOrDefault(c => c.id == id)?.PhotoPath;
        }

        public void Dispose() { }
    }

}
