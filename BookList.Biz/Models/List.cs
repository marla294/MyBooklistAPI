using System;
namespace BookList.Biz.Models
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List()
        {
            Id = 0;
            Name = "";
        }

        public List(int id, string name) {
            Id = id;
            Name = name;
        }

    }
}
