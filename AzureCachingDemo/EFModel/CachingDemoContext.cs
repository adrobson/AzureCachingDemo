using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AzureCachingDemo.EFModel
{
    public partial class CachingDemoContext : DbContext
    {
        public virtual DbSet<Model> Model { get; set; }
        public virtual DbSet<ModelData> ModelData { get; set; }

        public CachingDemoContext(DbContextOptions<CachingDemoContext> options) :base(options)
        {

        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer(@"Server=.\;Database=CachingDemo;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ModelData>(entity =>
            {
                entity.HasOne(d => d.Model)
                    .WithMany(p => p.ModelData)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelData_Model");
            });
        }
    }
}
