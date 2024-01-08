using Microsoft.EntityFrameworkCore;
using SudokuApp.Core.Models;

namespace SudokuApp.Core.Contexes;

public class SudokuContext : DbContext
{
  public DbSet<SolutionHead> SolutionHeads { get; set; }
  public DbSet<SolutionItem> SolutionItems { get; set; }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SudokuDb.db");
    optionsBuilder.UseSqlite($"Data Source={dbFile}");

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

  }
}
