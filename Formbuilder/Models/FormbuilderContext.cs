using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Formbuilder.Models;

public partial class FormbuilderContext : DbContext
{
    public FormbuilderContext()
    {
    }

    public FormbuilderContext(DbContextOptions<FormbuilderContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FormFiledMap> FormFiledMaps { get; set; }

    public virtual DbSet<FormsMaster> FormsMasters { get; set; }

    public virtual DbSet<TblField> TblFields { get; set; }

    public virtual DbSet<TblFieldOption> TblFieldOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FormFiledMap>(entity =>
        {
            entity.HasKey(e => e.FormFiledMapId).HasName("PK__FormFile__938A073BE4DFCA7D");

            entity.ToTable("FormFiledMap");

            entity.HasOne(d => d.Field).WithMany(p => p.FormFiledMaps)
                .HasForeignKey(d => d.FieldId)
                .HasConstraintName("FK_FormFiledMap_tblField");

            entity.HasOne(d => d.FieldOption).WithMany(p => p.FormFiledMaps)
                .HasForeignKey(d => d.FieldOptionId)
                .HasConstraintName("FK_FormFiledMap_tblFieldOption");

            entity.HasOne(d => d.FormsMaster).WithMany(p => p.FormFiledMaps)
                .HasForeignKey(d => d.FormsMasterId)
                .HasConstraintName("FK_FormFiledMap_FormsMaster");
        });

        modelBuilder.Entity<FormsMaster>(entity =>
        {
            entity.HasKey(e => e.FormsMasterId).HasName("PK__FormsMas__BC05765155160DDB");

            entity.ToTable("FormsMaster");

            entity.Property(e => e.FormName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblField>(entity =>
        {
            entity.HasKey(e => e.FieldId).HasName("PK__tblField__C8B6FF072FE9F580");

            entity.ToTable("tblField");

            entity.Property(e => e.FieldName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InputType)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblFieldOption>(entity =>
        {
            entity.HasKey(e => e.FieldOptionId).HasName("PK__tblField__0C245BBC04E07372");

            entity.ToTable("tblFieldOption");

            entity.Property(e => e.OptionTitle)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.Field).WithMany(p => p.TblFieldOptions)
                .HasForeignKey(d => d.FieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblFieldOption_tblField");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
