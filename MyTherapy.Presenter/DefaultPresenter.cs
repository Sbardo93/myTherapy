using MyTherapy.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTherapy.Presenter
{
    public class DefaultPresenter
    {
        public static List<object> GetUsers()
        {
            return DataManager.GetUsers().ToList<object>();
        }
    }
}
