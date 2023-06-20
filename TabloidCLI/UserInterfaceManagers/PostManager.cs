using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
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
            Console.WriteLine("Post Menu:");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 6) Search by Author");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new PostDetailManager(this, _connectionString, post.Id); //return new postDetailManager
                    }
                case "3":
                    AddPost();
                    return this;
                case "4":
                    EditPost();
                    return this;
                case "5":
                    RemovePost();
                    return this;
                case "6":
                    SearchByAuthor();
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
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"{post.Title} URL: {post.Url}");
            }
        }

        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a post: ";
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

        private void SearchByAuthor()
        {
            Console.WriteLine("Input the author's ID");
            List<Author> authors = _authorRepository.GetAll();
            foreach (Author author in authors)
            {
                Console.WriteLine($"{author.Id}) {author.FirstName} {author.LastName}");
            }
            Console.Write("> ");
            int inputedId = int.Parse(Console.ReadLine());
            List<Post> posts = _postRepository.GetByAuthor(inputedId);
            foreach (Post post in posts)
            {
                if (inputedId == post.Author.Id)
                {
                    Console.WriteLine($"{post.Title} Written By: {post.Author.FirstName} {post.Author.LastName} URL: {post.Url}");
                };
            }
        }

        private void AddPost()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.Url = Console.ReadLine();

            Console.Write("Date: ");
            string publicationDateTime = Console.ReadLine();
            post.PublishDateTime = DateTime.ParseExact(publicationDateTime, "dd/MM/yyyy", null);

            Console.WriteLine("Select which Author wrote this post by ID");
            List<Author> authors = _authorRepository.GetAll();
            foreach (Author author in authors)
            {
                Console.WriteLine($"{author.Id}) {author.FirstName} {author.LastName}");
            }
            Console.Write("> ");
            int authorid = int.Parse(Console.ReadLine());
            foreach (Author author in authors)
            {
                if (authorid == author.Id)
                {
                    Author author1 = new Author();
                    {
                        author1.Id = author.Id;
                        author1.FirstName = author.FirstName;
                        author1.LastName = author.LastName;
                        author1.Bio = author.Bio;
                    }
                    post.Author = author1;
                }
            }

            Console.WriteLine("Select which Blog this post is from by ID");
            List<Blog> blogs = _blogRepository.GetAll();
            foreach (Blog blog in blogs)
            {
                Console.WriteLine($"{blog.Id}) Title: {blog.Title}");
            }
            Console.Write("> ");
            int blogId = int.Parse(Console.ReadLine());
            foreach (Blog blog in blogs)
            {
                if (blogId == blog.Id)
                {
                    Blog blog1 = new Blog();
                    {
                        blog1.Id = blog.Id;
                        blog1.Title = blog.Title;
                        blog1.Url = blog.Url;
                    }
                    post.Blog = blog1;
                }
            }
            _postRepository.Insert(post);
        }

        private void RemovePost()
        {
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
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

            //Title Change
            Console.Write("New title (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrEmpty(title))
            {
                postToEdit.Title = title;
            }

            //Url Change
            Console.Write("New url (blank to leave unchanged): ");
            string url = Console.ReadLine();
            if (!string.IsNullOrEmpty(url))
            {
                postToEdit.Url = url;
            }

            //PublicationDateTime Change
            Console.Write("New publication date DD/MM/YYYY (blank to leave unchanged): ");
            string publicationDateTime = Console.ReadLine();
            if (!string.IsNullOrEmpty(publicationDateTime))
            {
                postToEdit.PublishDateTime = DateTime.ParseExact(publicationDateTime,"dd/MM/yyyy", null);
            }

            //Author Change
            Console.Write("New author (blank to leave unchanged): ");
            string Author = Console.ReadLine();
            if (!string.IsNullOrEmpty(Author))
            {
                postToEdit.Author.Id = int.Parse(Author);
            }

            //Blog Change
            Console.Write("New blog (blank to leave unchanged): ");
            string Blog = Console.ReadLine();
            if (!string.IsNullOrEmpty(Blog))
            {
                postToEdit.Blog.Id = int.Parse(Blog);
            }

            _postRepository.Update(postToEdit);

        }
    }
}
