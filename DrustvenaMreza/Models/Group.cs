namespace DrustvenaMreza.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public Group(int id, string name, DateTime dateCreated)
        {
            Id = id;
            Name = name;
            DateCreated = dateCreated;
        }
    }
}
