using Heurystyka.Domain;
using Microsoft.EntityFrameworkCore;


namespace Heurystyka.Infrastructure
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) :base(options) { }
        public DbSet<AlgorithmResult> AlgorithmResults { get; set; }
        public DbSet<ReportMultiple> ReportMultiples { get; set; }

        public DbSet<AlgorithmParameter> AlgorithmParameters { get; set; }
        public DbSet<ReportSingle> ReportSingles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AlgorithmResult>()
                .HasMany(ar => ar.Parameters)               
                .WithOne()                                   
                .OnDelete(DeleteBehavior.Cascade);          


            modelBuilder.Entity<AlgorithmResult>()
                .ToTable("AlgorithmResults");
            modelBuilder.Entity<AlgorithmParameter>()
    .ToTable("AlgorithmParameters");

            modelBuilder.Entity<ReportMultiple>().HasMany(ar => ar.Reports).WithOne().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReportSingle>().ToTable("ReportMultiple");
            modelBuilder.Entity<ReportSingle>().ToTable("ReportSingle");

        }
    }
}
