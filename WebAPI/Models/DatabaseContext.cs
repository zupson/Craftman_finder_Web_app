using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

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

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Town> Towns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DatabaseConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contractor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contract__3214EC07BCE23283");

            entity.ToTable("Contractor");

            entity.HasIndex(e => e.PersonId, "UQ__Contract__AA2FFBE46B474297").IsUnique();

            entity.Property(e => e.CompanyName).HasMaxLength(250);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsFreelancer).HasDefaultValue(false);

            entity.HasOne(d => d.JobType).WithMany(p => p.Contractors)
                .HasForeignKey(d => d.JobTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contracto__JobTy__49C3F6B7");

            entity.HasOne(d => d.Person).WithOne(p => p.Contractor)
                .HasForeignKey<Contractor>(d => d.PersonId)
                .HasConstraintName("FK__Contracto__Perso__48CFD27E");
        });

        modelBuilder.Entity<ContractorLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contract__3214EC07D603E5B3");

            entity.ToTable("ContractorLocation");

            entity.HasOne(d => d.Contractor).WithMany(p => p.ContractorLocations)
                .HasForeignKey(d => d.ContractorId)
                .HasConstraintName("FK__Contracto__Contr__571DF1D5");

            entity.HasOne(d => d.Location).WithMany(p => p.ContractorLocations)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Contracto__Locat__5812160E");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC078ED57CBE");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobAppli__3214EC0789C0102F");

            entity.ToTable("JobApplication");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.JobPost).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.JobPostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobApplic__JobPo__5DCAEF64");

            entity.HasOne(d => d.Person).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobApplic__Perso__5EBF139D");
        });

        modelBuilder.Entity<JobPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobPost__3214EC07D031ECDF");

            entity.ToTable("JobPost");

            entity.HasOne(d => d.ContractorLocation).WithMany(p => p.JobPosts)
                .HasForeignKey(d => d.ContractorLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JobPost__Contrac__5AEE82B9");
        });

        modelBuilder.Entity<JobType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobType__3214EC071F97631A");

            entity.ToTable("JobType");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07148CD026");

            entity.ToTable("Location");

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Town).WithMany(p => p.Locations)
                .HasForeignKey(d => d.TownId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Location__TownId__534D60F1");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC0794CCEB59");

            entity.ToTable("Permission");

            entity.Property(e => e.Name).HasMaxLength(250);

            entity.HasMany(d => d.Roles).WithMany(p => p.Permissions)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolePermi__RoleI__3C69FB99"),
                    l => l.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolePermi__Permi__3B75D760"),
                    j =>
                    {
                        j.HasKey("PermissionId", "RoleId").HasName("PK__RolePerm__570957CE7248C813");
                        j.ToTable("RolePermission");
                    });
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Person__3214EC072CA3CFD3");

            entity.ToTable("Person");

            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.PasswordHash).HasMaxLength(250);
            entity.Property(e => e.PasswordSalt).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.SecurityToken).HasMaxLength(250);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasMany(d => d.Roles).WithMany(p => p.People)
                .UsingEntity<Dictionary<string, object>>(
                    "PersonRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PersonRol__RoleI__4316F928"),
                    l => l.HasOne<Person>().WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PersonRol__Perso__4222D4EF"),
                    j =>
                    {
                        j.HasKey("PersonId", "RoleId").HasName("PK__PersonRo__128057047B80BB6D");
                        j.ToTable("PersonRole");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC075798B715");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Town>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Town__3214EC07E9EF4606");

            entity.ToTable("Town");

            entity.Property(e => e.Name).HasMaxLength(250);

            entity.HasOne(d => d.Country).WithMany(p => p.Towns)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Town__CountryId__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
