namespace DUTAdmin.Models
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class Context : DbContext
    {
        public Context() : base("Context")
        {
        }

        public DbSet<Student> Students { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}