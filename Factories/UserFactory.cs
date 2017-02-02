using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;
using Microsoft.Extensions.Options;

namespace theWall.Factory
{   
    
    public class UserFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
    
        public UserFactory(IOptions<MySqlOptions> conf) {
                mysqlConfig = conf;
        }
    
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }

        public void Add(User item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO User (fname, lname, email, pwd, created_at, updated_at) VALUES (@fname, @lname, @email, @pwd, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }

        public User FindByEmail(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM User WHERE email = @Email", new { Email = email }).FirstOrDefault();
            }
        }

        // public User FindByID(int id)
        // {
        //     using (IDbConnection dbConnection = Connection)
        //     {
        //         dbConnection.Open();
        //         return dbConnection.Query<User>("SELECT * FROM User WHERE userId = @Id", new { Id = id }).FirstOrDefault();
        //     }
        // }
        // public User FindById(int id)
        // {
        //     using (IDbConnection dbConnection = Connection)
        //     {
        //         dbConnection.Open();
        //         var query =
        //         @"
        //         SELECT * FROM User WHERE id = @Id;
        //         SELECT * FROM Quote WHERE quote_userId = @Id;";

        //         using (var multi = dbConnection.QueryMultiple(query, new { Id = id }))
        //         {
        //             var user = multi.Read<User>().Single();
        //             user.quotes = multi.Read<Quote>().ToList();
        //             return user;
        //         }
        //     }
        // }


    }
}