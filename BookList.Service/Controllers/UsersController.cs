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

        // id == userToken
        public User Get(string id)
        {
            return UserFactory.LoadSingleByToken(id);
        }

        // returns the userToken if the user is found
        public string Post([FromBody]UserData value)
        {
            if (value.Name != "")
            {
                return UserFactory.CreateNewUser(Db, value.Name, value.Username, value.Password);
            }

            else
            {
                return UserFactory.ConfirmUserPassword(value.Username, value.Password) ?
                                  UserFactory.LoadSingle(value.Username).Token :
                                  null;
            }

        }
    }
}
