using Dapper;
using Dz4.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using static System.Reflection.Metadata.BlobBuilder;

namespace Dz4.Services
{
    public interface ILibraryService
    {
        public Task<User> CreateUserAsync(User user);
        public Task<Book> CreateBookAsync(Book book);
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<IEnumerable<Book>> GetAllBooksAsync();
        public Task<IEnumerable<BookHistory>> GetAllBookHistoryAsync();
        public Task<User> UpdateUserAsync(int id, User user);
        public Task<Book> UpdateBookAsync(int id, Book book);
        public Task<User> delUserAsync(int id);
        public Task<Book> delBookAsync(int id);
        public Task<BookHistory> delBookHistoryAsync(int id);
        public Task<BookHistory> takeReturnBook(int userId, int bookId);

    }
    public class LibraryService : ILibraryService
    {
        private readonly string connectionString = @"Server=DESKTOP-1BJ6LFG;Database=LibraryDB;Integrated Security=True;TrustServerCertificate=True;";
        private static readonly object _lock = new();            
        private static readonly Mutex _mutex = new();             
        private static volatile bool _isProcessing = false;

        public async Task<Book> CreateBookAsync(Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO Books (Title, Author, IsAvailable, DueDate) VALUES (@Title, @Author, @IsAvailable, @DueDate)";
                await conn.ExecuteAsync(query, book);
                await conn.CloseAsync();
                return book;
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO Users (Name) VALUES (@Name)";
                await conn.ExecuteAsync(query, user);
                await conn.CloseAsync();
                return user;
            }
        }

        public async Task<Book> delBookAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var book = await conn.QueryFirstOrDefaultAsync<Book>(
                    "SELECT * FROM Books WHERE Id = @Id", new { Id = id });

                var deleteHistoryQuery = "DELETE FROM BookHistory WHERE BookId = @Id";
                await conn.ExecuteAsync(deleteHistoryQuery, new { Id = id });

                var deleteBookQuery = "DELETE FROM Books WHERE Id = @Id";
                await conn.ExecuteAsync(deleteBookQuery, new { Id = id });

                await conn.CloseAsync();
                return book;
            }
        }

        public async Task<BookHistory> delBookHistoryAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var history = await conn.QueryFirstOrDefaultAsync<BookHistory>(
                    "SELECT * FROM BookHistory WHERE Id = @Id", new { Id = id });
                var query = "DELETE FROM BookHistory WHERE Id = @Id";
                await conn.ExecuteAsync(query, new { Id = id });
                return history;
            }
        }

        public async Task<User> delUserAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var user = await conn.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
                var query = "DELETE FROM Users WHERE Id = @Id";
                await conn.ExecuteAsync(query, new { Id = id });
                return user;
            }
        }

        public async Task<IEnumerable<BookHistory>> GetAllBookHistoryAsync()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM BookHistory";
                var bookHistory = await conn.QueryAsync<BookHistory>(query);
                await conn.CloseAsync();
                return bookHistory;
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM Books";
                var books = await conn.QueryAsync<Book>(query);
                await conn.CloseAsync();
                return books;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM Users";
                var users = await conn.QueryAsync<User>(query);
                await conn.CloseAsync();
                return users;
            }
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var sql = @"
                UPDATE Books 
                SET Title = @Title,
                Author = @Author,
                IsAvailable = @IsAvailable,
                DueDate = @DueDate
                WHERE Id = @Id"; 

                await conn.ExecuteAsync(sql, new
                {
                    book.Title,
                    book.Author,
                    book.IsAvailable,
                    book.DueDate,
                    Id = id
                });

                var updatedBook = await conn.QueryFirstOrDefaultAsync<Book>(
                    "SELECT * FROM Books WHERE Id = @Id", new { Id = id });

                await conn.CloseAsync();
                return updatedBook;
            }
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var sql = @"UPDATE Users 
                    SET Name = @Name
                    WHERE Id = @Id";

                await conn.ExecuteAsync(sql, new { user.Name, Id = id });
                var user1 = await conn.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
                await conn.CloseAsync();
                return user1;
            }
        }
        public async Task<BookHistory> takeReturnBook(int userId, int bookId)
        {
            _mutex.WaitOne();

            try
            {
                if (_isProcessing)
                    throw new Exception("The operation is already being performed by another thread");

                _isProcessing = true;

                lock (_lock)
                {
                    return takeReturnBookInternal(userId, bookId).GetAwaiter().GetResult();
                }
            }
            finally
            {
                _isProcessing = false;
                _mutex.ReleaseMutex();
            }
        }
        private async Task<BookHistory> takeReturnBookInternal(int userId, int bookId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var book = await conn.QueryFirstOrDefaultAsync<Book>(
                    "SELECT * FROM Books WHERE Id = @Id", new { Id = bookId });

                if (book == null)
                    throw new Exception("Книга не найдена");

                bool newAvailability = !book.IsAvailable;
                string action = newAvailability ? "RETURN" : "TAKE";
                DateTime actionDate = DateTime.Now;
                DateTime? dueDate = newAvailability ? null : actionDate.AddDays(14);

                var updateQuery = @"
            UPDATE Books 
            SET IsAvailable = @IsAvailable,
                DueDate = @DueDate
            WHERE Id = @Id";

                await conn.ExecuteAsync(updateQuery, new
                {
                    IsAvailable = newAvailability,
                    DueDate = dueDate,
                    Id = bookId
                });

                var history = new BookHistory(bookId, userId, action, actionDate);

                var historyQuery = @"
            INSERT INTO BookHistory (BookId, UserId, Action, ActionDate)
            VALUES (@BookId, @UserId, @Action, @ActionDate)";

                await conn.ExecuteAsync(historyQuery, history);
                await conn.CloseAsync();

                return history;
            }
        }
    }
}
