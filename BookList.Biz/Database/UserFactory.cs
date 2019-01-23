using System;
using System.Text;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;
using System.Security.Cryptography;

namespace BookList.Biz.Database
{
    public static class UserFactory
    {
        // returns the id of the new user
        // if the username is already taken, returns null
        public static string CreateNewUser(IDbConnection dbConnection, string name, string username, string password)
        {
            if (LoadSingle(username) != null)
            {
                return null;
            }

            if (!CheckUsername(username) || !CheckPassword(password) || !CheckFirstname(name))
            {
                return null;
            }

            string userToken = GenerateUserToken();
            var hashedPwd = HashPassword(password);

            dbConnection.Insert("users", new KeyValuePair<string, object>[] {
                                Pairing.Of("name", $"{name}"),
                                Pairing.Of("username", $"{username.ToLower()}"),
                                Pairing.Of("password", $"{hashedPwd}"),
                                Pairing.Of("usertoken", $"{userToken}")
            }).Execute();

            return userToken;
        }

        public static List<User> LoadAll(IDbConnection dbConnection)
        {
            var userResultSet = dbConnection.Take("users").OrderBy("id").Execute();
            var users = new List<User>();

            for (var i = 0; i < userResultSet[0].Count; i++)
            {
                User user = Int32.TryParse(userResultSet[0][i], out int id)
                    ? new User(id, userResultSet[1][i], userResultSet[2][i], userResultSet[3][i], userResultSet[4][i])
                    : new User();

                users.Add(user);
            }

            return users;
        }

        public static User LoadSingle(string username)
        {
            return !CheckUsername(username)
                ? null
                : LoadAll(new PostgreSQLConnection())
                .FirstOrDefault<User>(user => user.Username == username);
        }

        // Returns null if userToken is not in the db or is invalid
        public static User LoadSingleByToken(string userToken)
        {
            return string.IsNullOrEmpty(userToken)
                ? null
                : LoadAll(new PostgreSQLConnection())
                .FirstOrDefault<User>(user => user.Token == userToken);
        }

        public static bool ConfirmUserPassword(string username, string password)
        {
            if (!CheckUsername(username) || !CheckPassword(password))
            {
                return false;
            }

            var hashedPwd = HashPassword(password);
            var user = LoadSingle(username.ToLower());

            if (user != null)
            {
                return user.Password == hashedPwd ? true : false;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteUser(IDbConnection dbConnection, string userToken)
        {
            if (!string.IsNullOrWhiteSpace(userToken)) {
                dbConnection.Delete("users").Where(Pairing.Of("userToken", userToken)).Execute();
            }
        }

        private static bool CheckUsername(string username)
        {
            return username.Length <= 40 && FactoryUtils.CheckInput(username, 7, 40, @"^[a-zA-Z0-9_.@-]*$") != null;
        }

        private static bool CheckPassword(string password)
        {
            return password.Length <= 40 && FactoryUtils.CheckInput(password, 7, 40) != null;
        }

        private static bool CheckFirstname(string firstname)
        {
            return firstname.Length <= 40 && FactoryUtils.CheckInput(firstname, 1, 40, @"^[a-zA-Z]*$") != null;
        }

        private static string HashPassword(string password)
        {
            string salt = "5%d2$#@asdrewq@334";
            string saltedPassword = password + salt;
            byte[] data = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString());
            }

            return sBuilder.ToString();
        }

        private static string GenerateUserToken()
        {
            var rando = new Random().Next(0, 1000000000).ToString();

            return HashPassword(rando);
        }
    }
}