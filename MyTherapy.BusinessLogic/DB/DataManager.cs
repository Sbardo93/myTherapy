using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MyTherapy.BusinessLogic.DB
{
    public class DataManager
    {
        public static string ConnectionString
        {
            get
            {
                return Properties.Settings.Default.MyTherapyDB;
            }
        }

        public static List<user> GetUsers()
        {
            using (MyTherapyDB db = new MyTherapyDB(ConnectionString))
            {
                var res = (from a in db.user select a).ToList();
                return res;
            }
        }
    }
}