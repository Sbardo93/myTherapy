using System.Data.Entity;
namespace MyTherapy.BusinessLogic
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class MyTherapyDB : DbContext
    {
        public MyTherapyDB(string dbService)
            : base("name=MyTherapyDB")
        {
            this.Database.Connection.ConnectionString = dbService;
        }
    }
}
