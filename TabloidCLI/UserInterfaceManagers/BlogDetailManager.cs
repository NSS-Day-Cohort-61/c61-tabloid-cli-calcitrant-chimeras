using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Repositories;
using TabloidCLI.Models;
using System.ComponentModel.Design;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class BlogDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private PostRepository _postRepository;
        private TagRepository _tagRepository;
        private int _blogId;
        public BlogDetailManager(IUserInterfaceManager parentUI, string connectionString, int blogId)
        {
            _parentUI = parentUI;
            _blogRepository = new BlogRepository(connectionString);
            _postRepository = new PostRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);
            _blogId = blogId;
        }

        public IUserInterfaceManager Execute()
        {
            Blog blog = _blogRepository.Get(_blogId);
            Console.WriteLine($"{blog.Title} Details");
            Console.WriteLine(" 1) View blog details");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Remove Tag");
            Console.WriteLine(" 4) View posts");
            Console.WriteLine(" 0) Go Back");
            Console.Write("> ");



            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    View();
                    return this;
                case "0":
                    return _parentUI;
                case "2":
                    AddTag();
                    return this;
                case "3":
                    RemoveTag();
                    return this;
                case "4":
                    ViewBlogPosts();
                    return this;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }

        }

        private void View()
        {
            Blog blog = _blogRepository.Get(_blogId);

            Console.WriteLine();
            Console.WriteLine($"Blog title: {blog.Title}");
            Console.WriteLine($"URL: {blog.Url}");
            if (blog.Tags.Count > 0)
            {
                Console.WriteLine($"All blog tags: ");
                foreach (Tag tag in blog.Tags)
                {
                    Console.WriteLine($"{tag.Name}");
                }
            }

            Console.WriteLine();
        }

        private void ViewBlogPosts()
        {
            Blog blog = _blogRepository.Get(_blogId);
            List<Post> posts = _postRepository.GetByBlog(blog.Id);
            Console.WriteLine();
            Console.WriteLine($"Posts for {blog.Title}: ");

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine(@$" {i + 1}) {post.Title} ({post.Url})");
                Console.WriteLine($"By: {post.Author.FirstName} {post.Author.LastName}");
                Console.WriteLine($"Posted on {post.PublishDateTime}");
            }
            Console.WriteLine();
        }

        private void AddTag()
        {
            Blog blog = _blogRepository.Get(_blogId);

            Console.WriteLine($"Which tag would you like to add to {blog.Title}?");
            List<Tag> tags = _tagRepository.GetAll();
            for (int i = 0; i < tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($" {i + 1}) {tag.Name}");
            }
            Console.Write("> ");
            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                Tag tag = tags[choice - 1];
                _blogRepository.InsertTag(blog, tag);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection. Won't add any tags.");
            }
        }

        private void RemoveTag()
        {
            Blog blog = _blogRepository.Get(_blogId);

            if (blog.Tags.Count > 0)
            {
                Console.WriteLine($"Which tag would you like to remove from {blog.Title}?");
                List<Tag> tags = blog.Tags;

                for (int i = 0; i < tags.Count; i++)
                {
                    Tag tag = tags[i];
                    Console.WriteLine($" {i + 1}) {tag.Name}");
                }
                Console.WriteLine("> ");

                string input = Console.ReadLine();
                try
                {
                    int choice = int.Parse(input);
                    Tag tag = tags[choice - 1];
                    _blogRepository.DeleteTag(blog.Id, tag.Id);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine("Invalid Selection. Won't remove any tags.");
                }
            }
            else
            {
                Console.WriteLine("This blog has no tags.");
                Console.WriteLine();
            }

        }
    }
}
