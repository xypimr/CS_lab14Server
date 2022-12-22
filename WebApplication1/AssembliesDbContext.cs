using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public partial class AssembliesDbContext : DbContext
{
    public AssembliesDbContext()
    {
    }

    public AssembliesDbContext(DbContextOptions<AssembliesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assembly> Assemblies { get; set; }

    public virtual DbSet<Detail> Details { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source = /Users/marmelad/RiderProjects/lab12/WebApplication1/DataBase/AssembliesDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasIndex(e => e.AssemblyId, "IX_Parts_AssemblyId");

            entity.HasIndex(e => e.DetailId, "IX_Parts_DetailId");

            entity.HasOne(d => d.Assembly).WithMany(p => p.Parts).HasForeignKey(d => d.AssemblyId);

            //entity.HasOne(d => d.Detail).WithMany(p => p.Parts).HasForeignKey(d => d.DetailId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
