using Dz4.Models;
using Dz4.Services;

namespace Dz4
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            ILibraryService libraryService = new LibraryService();

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


            var users = await libraryService.GetAllUsersAsync();
            foreach (var user in users) 
            {  
                Console.WriteLine(user); 
            }
            Console.WriteLine();
            var books = await libraryService.GetAllBooksAsync();
            foreach (var book in books) 
            { 
                Console.WriteLine(book);
            }
            Console.WriteLine();
            var histories = await libraryService.GetAllBookHistoryAsync();
            foreach (var h in histories) 
            { 
                Console.WriteLine(h); 
            }
        }
    }
}
