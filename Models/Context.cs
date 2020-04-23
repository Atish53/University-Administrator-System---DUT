namespace DUTAdmin.Models
{
    using System.Data.Entity;
    public class Context : DbContext
    {
        public Context() : base("name=Context")
        {

        }

        public DbSet<Student> Students { get; set; }

    }
}