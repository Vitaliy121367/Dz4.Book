namespace Dz4.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? DueDate { get; set; }
        public Book(int id, string title, string author, bool isAvailable, DateTime? dueDate)
        {
            Id = id;
            Title = title;
            Author = author;
            IsAvailable = isAvailable;
            DueDate = dueDate;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Author: {Author}, IsAvailable: {IsAvailable}, DueDate: {DueDate}";
        }
    }
}
