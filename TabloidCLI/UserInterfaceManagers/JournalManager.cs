﻿using System;
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
            Console.WriteLine(" 1) List Journals");
            Console.WriteLine(" 2) View a Journal");
            Console.WriteLine(" 3) Add Journal");
            Console.WriteLine(" 4) Edit Journal");
            Console.WriteLine(" 5) Delete Journal");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    View();
                    return this;
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
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

        private Journal Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Journal:";
            }

            Console.WriteLine(prompt);
            List<Journal> journals = _journalRepository.GetAll();

            for (int i = 0; i < journals.Count; i++)
            {
                Journal journal = journals[i];
                Console.WriteLine($" {i + 1}) {journal.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return journals[choice - 1];
            }
            catch (Exception)
            {
                Console.Write("Invalid Selection");
                return null;
            }
        }

        private void View() 
        {
            Journal journalToView = Choose("Which journal would you like to view?");
            if (journalToView == null)
            {
                return;
            }
            Console.WriteLine($"{journalToView.Title} - {journalToView.CreateDateTime.ToString("MM/dd/yyyy")}");
            Console.WriteLine(journalToView.Content);
        }

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

        private void Edit()
        {
            Journal journalToEdit = Choose("Which journal would you like to edit?");
            if (journalToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New journal title (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                journalToEdit.Title = title;
            }
            Console.Write("New journal content (blank to leave unchanged): ");
            string content = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(content))
            {
                journalToEdit.Content = content;
            }

            _journalRepository.Update(journalToEdit);
        }

        private void Remove()
        {
            Journal journalToRemove = Choose("Which journal would you like to remove?");
            if (journalToRemove != null)
            {
                _journalRepository.Delete(journalToRemove.Id);
            }
        }
    }
}