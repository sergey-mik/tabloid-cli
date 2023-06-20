using System;
using System.Collections.Generic;
using System.Linq;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class NoteManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private NoteRepository _noteRepository;
        private PostRepository _postRepository;
        private string _connectionString;
        private readonly int _postId;

        public NoteManager(IUserInterfaceManager parentUI, string connectionString, int postId)
        {
            _parentUI = parentUI;
            _noteRepository = new NoteRepository(connectionString);
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
            _postId = postId;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Note Management Menu");
            Console.WriteLine(" 1) List Notes");
            Console.WriteLine(" 2) Add Note");
            Console.WriteLine(" 3) Remove Note");
            Console.WriteLine(" 0) Return");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Notes List");
                    List();
                    Console.WriteLine();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
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
            List<Note> allNotes = _noteRepository.GetAll();
            List<Note> filteredNotes = allNotes.Where(note => note.Post.Id == _postId).ToList();
            foreach (Note note in filteredNotes)
            {
                Console.WriteLine(note);
            }
        }
        private void Add()
        {
            Console.WriteLine("Add Note");
            Note note = new Note();

            note.Post = _postRepository.Get(_postId);

            Console.Write("Title: ");
            note.Title = Console.ReadLine();

            Console.Write("Text content: ");
            note.Content = Console.ReadLine();

            note.CreateDateTime = DateTime.Now;

            _noteRepository.Insert(note);
        }

        private Note Choose(string prompt = null, int postId = 0)
        {
            if (prompt == null)
            {
                prompt = "Please choose a note: ";
            }
            Console.WriteLine(prompt);

            List<Note> notes = _noteRepository.GetAll();

            if (postId != 0)
            {
               notes = notes.Where(note => note.Post.Id == _postId).ToList();
            }

            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                Console.WriteLine($" {i + 1}) {note.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return notes[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
        private void Remove()
        {   
            Note noteToDelete = Choose("Which note would you like to remove?", _postId);
            if (noteToDelete != null)
            {
                _noteRepository.Delete(noteToDelete.Id);
            }

        }


    }
}