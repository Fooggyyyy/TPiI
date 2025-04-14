using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Text;
using DAL003;

public class Program
{
    private static void Main(string[] args)
    {
        using (IRepository repository = Repository.Create("Celebrities")) // Директория (в текущем каталоге приложения)
        {
            // Вывод всех знаменитостей
            foreach (Celebrity celebrity in repository.getAllCelebrities())
            {
                Console.WriteLine($"id = {celebrity.id}, Firstname = {celebrity.Firstname}, " +
                                  $"Surname = {celebrity.Surname}, PhotoPath = {celebrity.PhotoPath}");
            }

            // Получение знаменитости с id = 1
            Celebrity? celebrity1 = repository.getCelebrityByid(1);
            if (celebrity1 != null)
            {
                Console.WriteLine($"id = {celebrity1.id}, Firstname = {celebrity1.Firstname}, " +
                                  $"Surname = {celebrity1.Surname}, PhotoPath = {celebrity1.PhotoPath}");
            }

            // Получение знаменитости с id = 3
            Celebrity? celebrity3 = repository.getCelebrityByid(3);
            if (celebrity3 != null)
            {
                Console.WriteLine($"id = {celebrity3.id}, Firstname = {celebrity3.Firstname}, " +
                                  $"Surname = {celebrity3.Surname}, PhotoPath = {celebrity3.PhotoPath}");
            }

            Celebrity? celebrity7 = repository.getCelebrityByid(7);
            if (celebrity7 != null)
            {
                Console.WriteLine($"id = {celebrity7.id}, Firstname = {celebrity7.Firstname}, " +
                                  $"Surname = {celebrity7.Surname}, PhotoPath = {celebrity7.PhotoPath}");
            }

            Celebrity? celebrity222 = repository.getCelebrityByid(222);

            if (celebrity222 != null)
            {
                Console.WriteLine($"id = {celebrity222.id}, Firstname = {celebrity222.Firstname}, " +
                                  $"Surname = {celebrity222.Surname}, PhotoPath = {celebrity222.PhotoPath}");
            }
            else
            {
                Console.WriteLine("Not Found 2222");
            }

            foreach (Celebrity celebrity in repository.GetCelebrityBySurname("Chomsky"))
            {
                Console.WriteLine($"id = {celebrity.id}, Firstname = {celebrity.Firstname}, " +
                                  $"Surname = {celebrity.Surname}, PhotoPath = {celebrity.PhotoPath}");
            }

            foreach (Celebrity celebrity in repository.GetCelebrityBySurname("Knuth"))
            {
                Console.WriteLine($"id = {celebrity.id}, Firstname = {celebrity.Firstname}, " +
                                  $"Surname = {celebrity.Surname}, PhotoPath = {celebrity.PhotoPath}");
            }

            foreach (Celebrity celebrity in repository.GetCelebrityBySurname("XXXX"))
            {
                Console.WriteLine($"id = {celebrity.id}, Firstname = {celebrity.Firstname}, " +
                                  $"Surname = {celebrity.Surname}, PhotoPath = {celebrity.PhotoPath}");
            }

            Console.WriteLine($"PhotoPathByid = {repository.getPhotoPathByid(4)}");
            Console.WriteLine($"PhotoPathByid = {repository.getPhotoPathByid(6)}");
            Console.WriteLine($"PhotoPathByid = {repository.getPhotoPathByid(222)}");

        }
    }

}