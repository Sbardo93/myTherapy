using MySql.Data.MySqlClient;
using System.Linq;

namespace MyTherapy.BusinessLogic
{
    public class DataManager
    {
        public static bool TestConnectionLINQ(string connString)
        {
            using (MyTherapyContext db = new MyTherapyContext())
            {
                var res = (from a in db.user select a).ToList();
                return res.Any();
            }
        }

        public static bool TestConnection(string connString)
        {
            //MySqlConnection MyConnection = null;
            //MySqlDataReader MyReader = null;
            //// Create the SQL connection.
            //MyConnection = new MySqlConnection("server=5.189.165.60;port=3306;database=MyTherapy;user=user;password=user;");
            //MyConnection.Open();
            //// Create the command.
            //MySqlCommand MyCommand = new MySqlCommand("SELECT * FROM user", MyConnection);
            //// Execute the command
            //MyReader = MyCommand.ExecuteReader();
            //while (MyReader.Read())
            //{
            //    string a = MyReader.ToString();
            //}
            //// ...
            //MyReader.Close();
            //MyConnection.Close();
            return true;
        }
    }
}