using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Practic.Models;

namespace Practic;

public partial class PracticdataContext : DbContext
{
    public PracticdataContext()
    {
    }

    public PracticdataContext(DbContextOptions<PracticdataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Fabric> Fabrics { get; set; }

    public virtual DbSet<Firm> Firms { get; set; }

    public virtual DbSet<ForWhat> ForWhats { get; set; }

    public virtual DbSet<ServiceName> ServiceNames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=D:\\labs\\praktik\\Practic\\Practic\\practicdata.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("Application");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Price).HasColumnType("NUMERIC");

            entity.HasOne(d => d.Currency).WithMany(p => p.Applications)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Fabric).WithMany(p => p.Applications)
                .HasForeignKey(d => d.FabricId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Firm).WithMany(p => p.Applications)
                .HasForeignKey(d => d.FirmId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("Currency");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Fabric>(entity =>
        {
            entity.ToTable("Fabric");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodeOkrb).HasColumnName("CodeOKRB");
            entity.Property(e => e.CodeTnved).HasColumnName("CodeTNVED");

            entity.HasOne(d => d.ForWhat).WithMany(p => p.Fabrics)
                .HasForeignKey(d => d.ForWhatId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SetviceName).WithMany(p => p.Fabrics)
                .HasForeignKey(d => d.SetviceNameId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Firm>(entity =>
        {
            entity.ToTable("Firm");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<ForWhat>(entity =>
        {
            entity.ToTable("ForWhat");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<ServiceName>(entity =>
        {
            entity.ToTable("ServiceName");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
