using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BL.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contractor> Contractors { get; set; }

    public virtual DbSet<ContractorLocation> ContractorLocations { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<JobPost> JobPosts { get; set; }

    public virtual DbSet<JobType> JobTypes { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Town> Towns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DatabaseConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contractor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contract__3214EC07A124BB0F");

            entity.ToTable("Contractor");

            entity.HasIndex(e => e.PersonId, "UQ__Contract__AA2FFBE429784BC0").IsUnique();

            entity.Property(e => e.CompanyName).HasMaxLength(250);
            entity.Property(e => e.IsFreelancer).HasDefaultValue(false);

            entity.HasOne(d => d.JobType).WithMany(p => p.Contractors)
                .HasForeignKey(d => d.JobTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contractor_JobType");

            entity.HasOne(d => d.Person).WithOne(p => p.Contractor)
                .HasForeignKey<Contractor>(d => d.PersonId)
                .HasConstraintName("FK_Contractor_Person");
        });

        modelBuilder.Entity<ContractorLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contract__3214EC07C96CEF40");

            entity.ToTable("ContractorLocation");

            entity.HasOne(d => d.Contractor).WithMany(p => p.ContractorLocations)
                .HasForeignKey(d => d.ContractorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorLocation_Contractor");

            entity.HasOne(d => d.Location).WithMany(p => p.ContractorLocations)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractorLocation_Location");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC07F5D09CEB");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobAppli__3214EC072031C4E9");

            entity.ToTable("JobApplication");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.JobPost).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.JobPostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobApplication_JobPost");

            entity.HasOne(d => d.Person).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobApplication_Person");
        });

        modelBuilder.Entity<JobPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobPost__3214EC075ED1B1E4");

            entity.ToTable("JobPost");

            entity.HasOne(d => d.ContractorLocation).WithMany(p => p.JobPosts)
                .HasForeignKey(d => d.ContractorLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobPost_ContractorLocation");
        });

        modelBuilder.Entity<JobType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobType__3214EC0759DFE732");

            entity.ToTable("JobType");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07AA19C5EB");

            entity.ToTable("Location");

            entity.Property(e => e.PostalCode).HasMaxLength(50);

            entity.HasOne(d => d.Town).WithMany(p => p.Locations)
                .HasForeignKey(d => d.TownId)
                .HasConstraintName("FK_Location_Town");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC0775170015");

            entity.Property(e => e.Message).HasMaxLength(250);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Person__3214EC075D4E37BC");

            entity.ToTable("Person");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.PasswordHash).HasMaxLength(250);
            entity.Property(e => e.PasswordSalt).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasMany(d => d.Roles).WithMany(p => p.People)
                .UsingEntity<Dictionary<string, object>>(
                    "PersonRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PersonRole_Role"),
                    l => l.HasOne<Person>().WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PersonRole_Person"),
                    j =>
                    {
                        j.HasKey("PersonId", "RoleId");
                        j.ToTable("PersonRole");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0730B5CCDC");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Town>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Town__3214EC07D54C5E90");

            entity.ToTable("Town");

            entity.Property(e => e.Name).HasMaxLength(250);

            entity.HasOne(d => d.Country).WithMany(p => p.Towns)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Town_Country");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
