using fitnessBudyApi.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();

    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // globalno lowercase za vse tabele in stolpce
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()!.ToLower());
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToLower());
            }
        }
    }
}
