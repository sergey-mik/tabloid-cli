using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private TagRepository _tagRepository;
        private int _postId;
        private string _connectionString;

        public PostDetailManager(IUserInterfaceManager parentUI, string connectionString, int postId)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);
            _postId = postId;
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Post post = _postRepository.Get(_postId);
            Console.WriteLine($"{post.Title} Details");
            Console.WriteLine(" 1) View");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Remove Tag");
            Console.WriteLine(" 4) Note Management");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    View();
                    return this;
                case "2":
                    CreatePostTag();
                    return this;
                case "3":
                    DeletePostTag();
                    return this;
                case "4":
                    return new NoteManager(this, _connectionString, post.Id);
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void View()
        {
            Post post = _postRepository.Get(_postId);
            List<Tag> matchingTags = _tagRepository.GetPostTags(_postId);
            foreach ( Tag tag in matchingTags )
            {
                Console.WriteLine($"Tag: {tag.Name}");
            }
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"URL: {post.Url}");
            Console.WriteLine($"Publication Date: {post.PublishDateTime}");
        }

        private void CreatePostTag()
        {
            List<Tag> allTags = _tagRepository.GetAll();

            for (int i = 0; i < allTags.Count; i ++)
            {
                Tag tag = allTags[i];
                Console.WriteLine($" {i + 1}) {tag.Name}");
            }
            Console.Write("> ");
            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
            
            var userChoice = allTags[choice - 1];
                _postRepository.InsertPostTag(_postId, userChoice.Id);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection");
                
            }
        }

        public void DeletePostTag()
        {
            List<PostTag> postTags = _postRepository.GetPosttags(_postId);
            Console.WriteLine("Please choose a tag to delete from this post:");
            for(int i = 0; i < postTags.Count;i ++) 
            {
                PostTag postTag = postTags[i];
                Console.WriteLine($" {i + 1}) {postTag.Tag.Name}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                PostTag postTagToDelete = postTags[choice - 1];
                _postRepository.DeletePosttag(postTagToDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
            }
        }
    }
}
