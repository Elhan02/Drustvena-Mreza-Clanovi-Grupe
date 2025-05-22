using DrustvenaMreza.Models;
using System.Globalization;

namespace DrustvenaMreza.Repositories
{
    public class UserRepository
    {
        private const string filePath = "data/korisnici.csv";
        private const string clanstvaPath = "data/clanstva.csv";
        public static Dictionary<int, User> data;

        public UserRepository()
        {
            GroupRepository groupRepository = new GroupRepository();

            if (data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            data = new Dictionary<int, User>();
            List<string> lines = File.ReadAllLines(filePath).ToList();
            foreach (string line in lines)
            {
                string[] attributes = line.Split(",");
                int id = int.Parse(attributes[0]);
                string userName = attributes[1];
                string name = attributes[2];
                string lastname = attributes[3];
                DateTime birthdate = DateTime.ParseExact(attributes[4], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                User newUser = new User(id, userName, name, lastname, birthdate);
                data[newUser.Id] = newUser;
            }
            List<string> linesMemberships = File.ReadAllLines(clanstvaPath).ToList();
            foreach (string line in linesMemberships)
            {
                string[] attributes = line.Split(",");
                int idUser = int.Parse(attributes[0]);
                int idGropup = int.Parse(attributes[1]);
                data[idUser].Groups.Add(GroupRepository.Data[idGropup]);               
            }
           
        }

        public void Save() 
        {
            List<string> lines = new List<string>();
            List<string> linesMemberships = new List<string>();
            foreach (User user in data.Values)
            {
                lines.Add($"{user.Id},{user.UserName},{user.Name},{user.Lastname},{user.Birthdate.ToString("yyyy-MM-dd")}");
                if (user.Groups.Count == 0)
                {
                    continue;
                }
                foreach (Group group in user.Groups) 
                {
                    linesMemberships.Add($"{user.Id},{group.Id}");
                }
            }
            File.WriteAllLines(filePath, lines);
            File.WriteAllLines(clanstvaPath, linesMemberships);
        }        
    }
}
