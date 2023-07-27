using ContactInfo.App.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactInfo.App.Repositories;

public class ContactInfoContext: DbContext
{
    public virtual DbSet<User>? Users { get; set; }
    public virtual DbSet<Person>? Persons { get; set; }
    public virtual DbSet<Contact>? Contacts { get; set; }

    private string? DbPath { get; }
    private readonly Action<DbContextOptionsBuilder> _configStrategy = (_) => {};

    public ContactInfoContext(DbContextOptions contextOptions): base(contextOptions) {}

    public ContactInfoContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "contact.db");
        _configStrategy = (options) =>
        {
            options.UseSqlite($"Data Source={DbPath}");
        };
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        _configStrategy(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}