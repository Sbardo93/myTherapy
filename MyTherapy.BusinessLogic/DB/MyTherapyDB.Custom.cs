using System.Data.Entity;
namespace MyTherapy.BusinessLogic.DB
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class MyTherapyDB : DbContext
    {
        public MyTherapyDB(string dbService)
            : base(dbService)
        {
            //this.Database.Connection.ConnectionString = dbService;
        }
    }
}
