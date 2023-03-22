using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorRepository _authorRepository;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Add Post");
            Console.WriteLine(" 3) Edit Post");
            Console.WriteLine(" 4) Remove Post");
            Console.WriteLine(" 5) Post Details");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    AddPost();
                    return this;
                case "3":
                    EditPost();
                    return this;
                case "4":
                    DeletePost();
                    return this;
                case "5":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    {
                        return new PostDetailManager(this, _connectionString, post.Id);
                    }
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"{post.Title} -- {post.Url}");
            }
        }
        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void AddPost()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Post Title: ");
            post.Title = Console.ReadLine();

            Console.Write("Post URL: ");
            post.Url = Console.ReadLine();

            Console.Write("Publication Date (YYYY-MM-DD): ");
            post.PublishDateTime = DateTime.Parse(Console.ReadLine());

            Author author = ChooseAuthor();
            post.Author = author;

            Blog blog = ChooseBlog();
            post.Blog = blog;

            _postRepository.Insert(post);
        }

        private Author ChooseAuthor(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Author:";
            }

            Console.WriteLine(prompt);

            List<Author> authors = _authorRepository.GetAll();

            for (int i = 0; i < authors.Count; i++)
            {
                Author author = authors[i];
                Console.WriteLine($" {i + 1}) {author.FullName}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return authors[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private Blog ChooseBlog(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a blog";
            }

            Console.WriteLine(prompt);

            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count;i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1} {blog.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch(Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void EditPost()
        {
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New title (blank to leave unchanged): ");
            string postTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(postTitle))
            {
                postToEdit.Title = postTitle;
            }
            Console.Write("New url (blank to leave unchanged): ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }
            Console.Write("New publish date YYYY-MM-DD (blank to leave unchanged): ");
            DateTime publishDate = DateTime.Parse(Console.ReadLine());
            if (publishDate != default)
            {
                postToEdit.PublishDateTime = publishDate;
            }
            Console.WriteLine("New Author");
            Author postAuthor = ChooseAuthor();
            postToEdit.Author.Id = postAuthor.Id;
            Console.WriteLine("New blog");
            Blog postBlog = ChooseBlog();
            postToEdit.Blog.Id = postBlog.Id;

            _postRepository.Update(postToEdit);
        }

        private void DeletePost()
        {
            Post postToDelete = Choose("Which post would you like to delete?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }
        }

    }
}
