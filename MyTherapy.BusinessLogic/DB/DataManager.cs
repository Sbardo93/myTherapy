using System.Linq;

namespace MyTherapy.BusinessLogic
{
    public class DataManager
    {
        public static bool TestConnectionLINQ(string connString)
        {
            using (MyTherapyDB db = new MyTherapyDB(connString))
            {
                var res = (from a in db.user select a).ToList();
                return res.Any();
            }
        }
    }
}