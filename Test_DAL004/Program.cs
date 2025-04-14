﻿using System;
using System.Diagnostics;
using DAL004;

class Programm
{
    public static void Main(string[] args)
    {
        using (IRepository repository = Repository.Create("Celebrities")) 
        {
            void print(string label)
            {
                Console.WriteLine("--- " + label + " ----------------");
                foreach (Celebrity celebrity in repository.getAllCelebrities())
                {
                    Console.WriteLine($"Id = {celebrity.id}, Firstname = {celebrity.Firstname}, " +
                                      $"Surname = {celebrity.Surname}, PhotoPath = {celebrity.PhotoPath} ");
                }
            }

            print("start");

            int? testdel1 = repository.addCelebrity(new Celebrity(0, "TestDel1", "TestDel1", "Photo/TestDel01.jpg"));
            int? testdel2 = repository.addCelebrity(new Celebrity(0, "TestDel2", "TestDel2", "Photo/TestDel02.jpg"));
            int? testupd1 = repository.addCelebrity(new Celebrity(0, "TestUpd1", "TestUpd1", "Photo/TestUpd1.jpg"));
            int? testupd2 = repository.addCelebrity(new Celebrity(0, "TestUpd2", "TestUpd2", "Photo/TestUpd2.jpg"));
            repository.SaveChanges();
            print("add 4");

            if(testdel1 != null) 
                if(repository.delCelebtrityById((int)testdel1))
                    Console.WriteLine($" delete {testdel1} ");
                else
                    Console.WriteLine($"delete {testdel1} error");
            if(testdel2 != null)
                if(repository.delCelebtrityById((int)(testdel2)))
                    Console.WriteLine($" delete {testdel2}");
                else
                    Console.WriteLine($"delete {1000} error");
            repository.SaveChanges();
            print("del 2");

            if (testupd1 != null)
                if (repository.updCelebrityById((int)testupd1, new Celebrity(0, "Updated1", "Updated1", "Photo/Updated1.jpg")))
                    Console.WriteLine($"update {testupd1} ");
                else
                    Console.WriteLine($"update {testupd1} error");
            if (testupd2 != null)
                if (repository.updCelebrityById((int)testupd2, new Celebrity(0, "Updated2", "Updated2", "Photo/Updated2.jpg")))
                    Console.WriteLine($"update {testupd2} ");
                else
                    Console.WriteLine($"update {testupd2} error");
            if (repository.updCelebrityById(1000, new Celebrity(0, "Update1000", "Update1000", "Photo/Update1000.jpg")))
                Console.WriteLine($" update {1000} ");
            else
                Console.WriteLine($" update {1000} error");
            repository.SaveChanges();
            print("upd 2");
        }

    }
}
