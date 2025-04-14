using System;
using System.Text;
using Newtonsoft.Json;

namespace DAL004
{
    public record Celebrity(int id, string Firstname, string Surname, string PhotoPath);
    public interface IRepository : IDisposable
    {
        string BasePath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebrityByid(int id);
        Celebrity[] GetCelebrityBySurname(string Surname);
        string? getPhotoPathByid(int id);
        int? addCelebrity(Celebrity celebrity);
        bool delCelebtrityById(int id);
        bool updCelebrityById(int id, Celebrity celebrity);
        int SaveChanges();
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

        public int? addCelebrity(Celebrity celebrity)
        {
            int maxId = AllCelebrity.Max(c => c.id) + 1;
            AllCelebrity.Add(new Celebrity(maxId, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath));

            return maxId;
        }

        public bool delCelebtrityById(int id)
        {
            var celebrityToRemove = AllCelebrity.FirstOrDefault(c => c.id == id);

            if (celebrityToRemove == null)
                return false; 

            AllCelebrity.Remove(celebrityToRemove);

            return true;
        }

        public bool updCelebrityById(int id, Celebrity celebrity)
        {
            //ОБНОВИТЬ PUT ID
            var existingCelebrity = AllCelebrity.FirstOrDefault(c => c.id == id);
            if (existingCelebrity == null)
                return false;

            var updatedCelebrity = existingCelebrity with
            {
                id = celebrity.id,
                Firstname = celebrity.Firstname,
                Surname = celebrity.Surname,
                PhotoPath = celebrity.PhotoPath
            };

            AllCelebrity.Remove(existingCelebrity);
            AllCelebrity.Add(updatedCelebrity);

            return true;
        }


        public int SaveChanges()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(AllCelebrity, Formatting.Indented);
                File.WriteAllText(BasePath, jsonString);

                return 1;
            }
            catch (Exception)
            {
                return 0; 
            }
        }

        public void Dispose() { }
    }

}
