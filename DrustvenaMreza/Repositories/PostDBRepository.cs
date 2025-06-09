using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;

namespace DrustvenaMreza.Repositories
{
    public class PostDBRepository
    {
        public readonly string connectionString;

        public PostDBRepository(IConfiguration configuration)
        {
            this.connectionString = configuration["ConnectionStrings:SQLiteConnection"];
        }

        public Post Create(Post post)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Posts(UserId, Content, Date)
                            VALUES(@UserId, @Content, @Date);
                            SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Content", post.Content);
                command.Parameters.AddWithValue("@Date", post.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@UserId", post.UserId);

                post.Id = Convert.ToInt32(command.ExecuteScalar());
                return post;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska se desila pri konekciji ili pri izvrsavanju neispravnih SQL naredbi:{ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija je otvorena vise puta ili nije otvorena: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
                throw;
            }
        }

        public List<Post> GetAll()
        {
            try
            {
                List<Post> posts = new List<Post>();
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT p.Id as PostId, p.Content, p.Date, u.Id as UserId, u.Username, u.Name, u.Surname, u.Birthday
                            FROM Posts p
                            INNER JOIN Users u on p.UserId = u.Id
                            ORDER BY u.Id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Post post = new Post
                    {
                        Id = Convert.ToInt32(reader["PostId"]),
                        Content = Convert.ToString(reader["Content"]),
                        Date = Convert.ToDateTime(reader["Date"]),
                        UserId = Convert.ToInt32(reader["UserId"])
                    };
                    post.User = new User
                    {
                        Id = Convert.ToInt32(reader["UserId"]),
                        UserName = Convert.ToString(reader["Username"]),
                        Name = Convert.ToString(reader["Name"]),
                        Lastname = Convert.ToString(reader["Surname"]),
                        Birthdate = Convert.ToDateTime(reader["Birthday"])
                    };
                    posts.Add(post);
                }
                return posts;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konekciji ili pri izvrsavanju nesipravnih SQL naredbi: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska se dogodila pri konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena vise  puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neocekivana greska: {ex.Message}");
                throw;
            }
        }
    }
}
