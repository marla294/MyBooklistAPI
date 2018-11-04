using System;
namespace BookList.Biz.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserDTO()
        {
            Id = 0;
            Name = "";
        }

        public UserDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
