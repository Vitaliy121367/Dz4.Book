
namespace Dz4.Models
{
    public class BookHistory
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime? ActionDate { get; set; }

        public BookHistory() { }

        public BookHistory(int bookId, int userId, string action, DateTime? actionDate)
        {
            BookId = bookId;
            UserId = userId;
            Action = action;
            ActionDate = actionDate;
        }

        public override string ToString()
        {
            return $"BookId: {BookId}, UserId: {UserId}, Action: {Action}, ActionDate: {ActionDate}";
        }
    }
}
