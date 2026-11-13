using MedicalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalApi.Data;

public class MedicalDbContext(DbContextOptions<MedicalDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Spravka> Spravki => Set<Spravka>();
    public DbSet<SpravkaType> SpravkaTypes => Set<SpravkaType>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Patient>().Property(p => p.Phone).HasMaxLength(20);
        modelBuilder.Entity<Patient>().Property(p => p.Address).HasMaxLength(200);
        modelBuilder.Entity<SpravkaType>().Property(t => t.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<SpravkaType>().Property(t => t.Price).HasPrecision(10, 2);
        modelBuilder.Entity<SpravkaType>().ToTable(t => t.HasCheckConstraint("CK_SpravkaTypes_Price", "\"Price\" > 0"));
        modelBuilder.Entity<Spravka>().Property(s => s.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Spravka>().HasOne<Patient>().WithMany().HasForeignKey(s => s.PatientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Spravka>().HasOne<SpravkaType>().WithMany().HasForeignKey(s => s.SpravkaTypeId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Appointment>().HasOne<Patient>().WithMany().HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Appointment>().Property(a => a.StartsAt).HasColumnType("timestamp without time zone");
        modelBuilder.Entity<Appointment>().Property(a => a.DoctorName).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Appointment>().Property(a => a.Specialty).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Appointment>().Property(a => a.Room).HasMaxLength(30).IsRequired();
    }
}

