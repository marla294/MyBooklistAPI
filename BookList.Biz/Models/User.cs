using System;
namespace BookList.Biz.Models
{
    public class User
    {
        int Id { get; set; }
        string Name { get; set; }

        public User()
        {
            Id = 0;
            Name = "";
        }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public UserDTO ToDTO()
        {
            return new UserDTO(Id, Name);
        }
    }
}
