
namespace RegisterDocs.Models
{
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Infrastructure;

  public partial class RegisterDocsDbContext : DbContext
  {
    public RegisterDocsDbContext()
        : base("RegisterDocs")
    {
      Database.CommandTimeout = 0;
    }

    public DbSet<Docs> Docs { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Docs>().ToTable("Docs", "public");
      base.OnModelCreating(modelBuilder);
    }
     
  }
}
