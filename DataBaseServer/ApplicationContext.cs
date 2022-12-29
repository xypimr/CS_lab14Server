using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataBaseServer;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Auditorium> Auditoriums { get; set; }

    public virtual DbSet<AuditoriumGroup> AuditoriumGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite(
            "Data Source = /Users/oldmash/RiderProjects/labsC#/CS_lab14/DataBaseServer/DataBase/AssembliesDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditoriumGroup>(entity =>
        {
            entity.HasIndex(e => e.BuildingId, "IX_AuditoriumGroups_BuildingId"); /////////////////////////////////

            entity.HasIndex(e => e.AuditoriumId, "IX_AuditoriumGroups_AuditoriumId");

            entity.HasOne(d => d.Building).WithMany(p => p.AuditoriumGroups).HasForeignKey(d => d.BuildingId);

            //entity.HasOne(d => d.Auditorium).WithMany(p => p.AuditoriumGroupViews).HasForeignKey(d => d.AuditoriumId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
