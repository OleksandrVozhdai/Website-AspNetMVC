using Microsoft.EntityFrameworkCore;

namespace LAB2_OOKP.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> UsersTable { get; set; }
        public DbSet<PizzaList> PizzaTable { get; set; }
        public DbSet<Purchase>  Purchases { get; set; }
        public DbSet<PizzaIngredients> PizzaIngredients { get; set; }
        public DbSet<Ingredients> IngredientsTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PizzaIngredients>()
                .HasKey(pi => new { pi.PizzaID, pi.IngredientID });
        }
    }
}
