using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using BookList.Biz.Models;
using BookList.Biz.Database;

public class UserData
{
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

namespace BookList.Service.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        PostgreSQLConnection Db { get; set; }

        public UsersController()
        {
            Db = new PostgreSQLConnection();
        }

        public int Get([FromBody]UserData value)
        {
            return UserFactory.LoadSingle(value.Username, value.Password);
        }

        // returns the id of the new list as a string
        public string Post([FromBody]UserData value)
        {
            return UserFactory.CreateNewUser(Db, value.Name, value.Username, value.Password);
        }
    }
}
