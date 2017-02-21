using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.EntityClient;
using System.Threading.Tasks;

namespace MyTherapy.BusinessLogic
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class MyTherapyDBContainer : DbContext
    {
        public MyTherapyDBContainer(string dbService)
          : base("MyTherapyDBContainer")
        {
            Database.SetInitializer<MyTherapyDBContainer>(null);

            Database.Connection.ConnectionString = dbService;
        }
    }
}
