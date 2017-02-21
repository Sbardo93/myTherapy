using System.Data.Entity;
namespace MyTherapy.BusinessLogic
{
    public partial class MyTherapyDB : DbContext
    {
        public MyTherapyDB()
            : base("name=MyTherapyDB")
        {
        }

        public virtual DbSet<user> user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Cognome)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.PartitaIVA)
                .IsUnicode(false);
        }
    }
}
