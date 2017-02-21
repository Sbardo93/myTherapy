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
    public partial class MyTherapyContext : DbContext
    {
        public MyTherapyContext(string dbService)
          : base("name=MyTherapyContext")
        {
            Database.SetInitializer<MyTherapyContext>(null);

            Database.Connection.ConnectionString = dbService;

        }
    }
}
