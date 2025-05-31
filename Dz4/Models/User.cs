namespace Dz4.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}";
        }
    }
}
