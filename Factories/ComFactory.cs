using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;


namespace theWall.Factory
{
    public class ComFactory : IFactory<Msg>
    {
        private string connectionString;
        public ComFactory()
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
        
        public void Add(Com item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO Com (comment, userid, msgid, created_at, updated_at) VALUES (@comment, @userid, @msgid, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }

        public IEnumerable<Com> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {   
                var query = "SELECT * FROM Com c JOIN User u ON c.userid WHERE c.userid = u.id ORDER BY c.created_at DESC";
                dbConnection.Open();                      
                var myComs = dbConnection.Query<Com, User , Com>(query, (com, user ) => { com.user = user; return com; }, splitOn: "id");
                return myComs;
            }
        }

     








//         public Msg FindById(int id){
//             using (IDbConnection dbConnection = Connection) {
//                 dbConnection.Open();
//                 var query =
//                 @"                                            *********Another way-ask someone *********
//                 SELECT * FROM Msg WHERE id = @Id;
//                 SELECT * FROM Com WHERE msgid = @Id;";

//                 using (var multi = dbConnection.QueryMultiple(query, new {Id = id})) {
//                     var msg = multi.Read<Msg>().Single();
//                     msg.comments = multi.Read<Com>().ToList();
//                     return msg;
//                 }
//             }
// }
        

        // public void deleteMsgsById(int id)
        // {
        //     using (IDbConnection dbConnection = Connection)
        //     {
        //         var query = $"DELETE FROM Msg WHERE id = {id}";
        //         dbConnection.Open();
        //         dbConnection.Execute(query);
        //     }
        // }


    }
}