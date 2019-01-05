namespace BookList.Biz.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public User()
        {
            Id = 0;
            Name = "";
            Username = "";
            Password = "";
            Token = "";
        }

        public User(int id, string name, string username, string password, string token)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            Token = token;
        }

    }
}
