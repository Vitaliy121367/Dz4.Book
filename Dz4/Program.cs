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

            //var newUser = await libraryService.CreateUserAsync(new User(1,"Alice"));
            //Console.WriteLine(newUser);

            //var newBook = await libraryService.CreateBookAsync(new Book(6, "1984", "George Orwell", true, null));
            //Console.WriteLine(newBook);

            //var updatedUser = await libraryService.UpdateUserAsync(1, new User(1, "Alice Updated"));
            //Console.WriteLine(updatedUser);

            //var updatedBook = await libraryService.UpdateBookAsync(6, new Book(6, "1984", "G. Orwell", true, null));
            //Console.WriteLine(updatedBook);

            //var history1 = await libraryService.takeReturnBook(1, 6);
            //Console.WriteLine(history1);

            //var history2 = await libraryService.takeReturnBook(1, 6);
            //Console.WriteLine(history2);

            //var deletedHistory = await libraryService.delBookHistoryAsync(1);
            //Console.WriteLine(deletedHistory);

            //var deletedBook = await libraryService.delBookAsync(6);
            //Console.WriteLine(deletedBook);

            //var deletedUser = await libraryService.delUserAsync(1);
            //Console.WriteLine(deletedUser);

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
