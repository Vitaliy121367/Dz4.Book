using Dz4.Models;
using Dz4.Services;
using System;

namespace Dz4
{
    public class Program
    {
        private static ILibraryService libraryService = new LibraryService();
        private static int listHeight = 0;
        static async Task Main(string[] args)
        {
            Timer _timer;

            _timer = new Timer(ShowUsersAndBooks, null, 0, 1000);
            Console.ReadKey();
        }
        static void ClearLine(int line)
        {
            Console.SetCursorPosition(0, line);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        static async void ShowUsersAndBooks(object state)
        {
            var users = await libraryService.GetAllUsersAsync();
            var books = await libraryService.GetAllBooksAsync();
            var histories = await libraryService.GetAllBookHistoryAsync();

            int line = 0;

            ClearLine(line);
            Console.SetCursorPosition(0, line++);
            Console.WriteLine("Users:");

            foreach (var user in users)
            {
                ClearLine(line);
                Console.SetCursorPosition(0, line++);
                Console.WriteLine(user);
            }

            ClearLine(line);
            Console.SetCursorPosition(0, line++);
            Console.WriteLine();

            ClearLine(line);
            Console.SetCursorPosition(0, line++);
            Console.WriteLine("Books:");

            foreach (var book in books)
            {
                ClearLine(line);
                Console.SetCursorPosition(0, line++);
                Console.WriteLine(book);
            }

            ClearLine(line);
            Console.SetCursorPosition(0, line++);
            Console.WriteLine();

            ClearLine(line);
            Console.SetCursorPosition(0, line++);
            Console.WriteLine("historie:");

            foreach (var h in histories)
            {
                ClearLine(line);
                Console.SetCursorPosition(0, line++);
                Console.WriteLine(h);
            }

            for (int i = line; i < listHeight; i++)
            {
                ClearLine(i);
            }

            listHeight = line;
        }
    }
}
