namespace BookList.Biz.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User()
        {
            Id = 0;
            Name = "";
            Username = "";
            Password = "";
        }

        public User(int id, string name, string username, string password)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
        }

    }
}
