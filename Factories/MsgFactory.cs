using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;

namespace theWall.Factory
{
    public class MsgFactory : IFactory<Msg>
    {
        private string connectionString;
        public MsgFactory()
        {
            connectionString = "server=localhost;userid=root;password=root;port=8889;database=thewall;SslMode=None";
        }
        internal IDbConnection Connection
        {
            get 
            {
                return new MySqlConnection(connectionString);
            }
        }
        
        public void Add(Msg item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO Msg (message, userid, created_at, updated_at) VALUES (@message, @userid, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }

        public IEnumerable<Msg> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                
                var query = "SELECT * FROM Msg m JOIN User u ON m.userid = u.id ORDER BY m.created_at DESC";
                dbConnection.Open();
                var myMsgs = dbConnection.Query<Msg, User , Msg>(query, (Msg, user ) => { Msg.user = user; return Msg; }, splitOn: "id");
                return myMsgs;
            }
        }

        // public IEnumerable<Msg> MsgsForUserById(int id)
        // {
        //     using (IDbConnection dbConnection = Connection)
        //     {
        //         var query = $"SELECT * FROM Msg JOIN User ON Msg_userId WHERE userId = Msg_userId AND userId = {id}";
        //         dbConnection.Open();
        //         var myMsgs = dbConnection.Query<Msg, User, Msg>(query, (Msg, user) => { Msg.Msg_userId = id; return Msg; });
        //         return myMsgs;
        //     }
        // }

        public void deleteMsgsById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = $"DELETE FROM Msg WHERE id = {id}";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }


    }
}
