using Microsoft.EntityFrameworkCore;

namespace HsmServer.UnitTest.Context
{
    public class DemoDbContext : DbContext 
    {
        public DemoDbContext()
        {
            
        }

        public DemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasKey(e => e.Id);
        }
    }
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}