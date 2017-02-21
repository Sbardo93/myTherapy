using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTherapy.BusinessLogic.Presenter
{
    public class DefaultPresenter
    {
        public static List<user> GetUsers()
        {
            return DB.DataManager.GetUsers();
        }
    }
}
