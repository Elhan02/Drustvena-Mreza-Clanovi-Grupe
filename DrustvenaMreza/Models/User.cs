namespace DrustvenaMreza.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();


        public User(int id, string userName, string name, string lastname, DateTime birthdate)
        {
            Id = id;
            UserName = userName;
            Name = name;
            Lastname = lastname;
            Birthdate = birthdate;
        }

        public User()
        {
        }        
    }
}
