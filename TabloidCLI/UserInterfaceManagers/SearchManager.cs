using System;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class SearchManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;

        public SearchManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _tagRepository = new TagRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Search Menu");
            Console.WriteLine(" 1) Search Blogs");
            Console.WriteLine(" 2) Search Authors");
            Console.WriteLine(" 3) Search Posts");
            Console.WriteLine(" 4) Search All");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Search("blogs");
                    return this;
                case "2":
                    Search("authors");
                    return this;
                case "3":
                    Search("posts");
                    return this;
                case "4":
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void Search(string model)
        {
            Console.Write("Tag > ");
            string tagName = Console.ReadLine();

            SearchResults<IResult> results = new SearchResults<IResult>();

            switch (model)
            {
                case "authors":
                    results = _tagRepository.SearchAuthors(tagName);
                    break;
                case "blogs":
                    results = _tagRepository.SearchBlogs(tagName);
                    break;
                case "posts":
                    results = _tagRepository.SearchPosts(tagName);
                    break;
                default:
                    Console.WriteLine("Invalid search query.");
                    break;
            }

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
            }
            else
            {
                results.Display();
            }

        }
        //private void SearchAuthors()
        //{
        //    Console.Write("Tag> ");
        //    string tagName = Console.ReadLine();

        //    SearchResults<Author> results = _tagRepository.SearchAuthors(tagName);

        //    if (results.NoResultsFound)
        //    {
        //        Console.WriteLine($"No results for {tagName}");
        //    }
        //    else
        //    {
        //        results.Display();
        //    }
        //}
    }
}