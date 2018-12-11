using System;
namespace BookList.Biz.Models
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }

        public List()
        {
            Id = 0;
            Name = "";
            Owner = null;
        }

        public List(int id, string name, User owner) {
            Id = id;
            Name = name;
            Owner = owner;
        }

    }
}
