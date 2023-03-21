using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Entries");
            Console.WriteLine(" 2) Add Entry");
            Console.WriteLine(" 3) Edit Entry");
            Console.WriteLine(" 4) Delete Entry");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Journal> journals = _journalRepository.GetAll();
            Console.WriteLine("All Journal Entries:");
            foreach (Journal journal in journals)
            {
                Console.WriteLine(journal.Title);
            }
        }

        //private Journal Choose(string prompt = null)
        //{
        //    if (prompt == null)
        //    {
        //        prompt = "Please choose a Journal:";
        //    }

        //    Console.WriteLine(prompt);
        //    List<Journal> journals = _journalRepository.GetAll();

        //    for (int i = 0; i < journals.Count; i++)
        //    {
        //        Journal journal = journals[i];
        //        Console.WriteLine($" {i + 1}) {journal.Title}");
        //    }
        //    Console.Write("> ");

        //    string input = Console.ReadLine();
        //    try
        //    {
        //        int choice = int.Parse(input);
        //        return journals[choice - 1];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write("Invalid Selection");
        //        return null;
        //    }
        //}

        private void Add()
        {
            Console.WriteLine("New Journal");
            Journal journal = new Journal();

            Console.Write("Journal Title: ");
            journal.Title = Console.ReadLine();

            Console.Write("Journal Content: ");
            journal.Content = Console.ReadLine();

            journal.CreateDateTime = DateTime.Now;

            _journalRepository.Insert(journal);
        }
    }
}