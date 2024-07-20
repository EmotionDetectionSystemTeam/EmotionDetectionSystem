using Microsoft.EntityFrameworkCore;
using EmotionDetectionSystem.DomainLayer.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EmotionDetectionSystem.DomainLayer.Repos;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Configuration;

namespace EmotionDetectionSystem.DataLayer
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EnrollmentSummary> EnrollmentSummaries { get; set; }
        public DbSet<EmotionData> EmotionData { get; set; }


        public DatabaseContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "SystemDB.db"));
            string absolutePath = $"Data Source={path};";
            absolutePath = "Data Source=market-db-server.database.windows.net;Initial Catalog=MarketDataBase;User ID=tamuzg@post.bgu.ac.il;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Authentication=ActiveDirectoryDefault;Application Intent=ReadWrite;Multi Subnet Failover=False;";
            //absolutePath = "Server=tcp:market-db-server.database.windows.net,1433;Initial Catalog=MarketDataBase;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";";
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseSqlServer($"Data Source={absolutePath}");
            optionsBuilder.UseSqlServer(absolutePath);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User inheritance (Table per Hierarchy)
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Teacher>("Teacher")
                .HasValue<Student>("Student");
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasKey(e => e.Email);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            });



            // Configure relationships
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Teacher)
                .WithMany()
                .HasForeignKey("TeacherId");

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasKey(e => e.LessonId);
                entity.Property(e => e.LessonId).IsRequired();
                entity.Property(e => e.LessonName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.EntryCode).HasMaxLength(50);

                entity.HasOne(e => e.Teacher)
                      .WithMany(t => t.Lessons)
                      .HasForeignKey("TeacherId")
                       .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Tags)
                .HasMaxLength(1000)
                      .HasConversion(
                          v => string.Join(',', v),
                          v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                      .Metadata.SetValueComparer(
                          new ValueComparer<List<string>>(
                              (c1, c2) => c1.SequenceEqual(c2),
                              c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                              c => c.ToList()
                          ));

                // Ignore the Viewers property for persistence
                entity.Ignore(e => e.Viewers);
                entity.Ignore(e => e.EnrollmentSummaryRepo);
            });

            modelBuilder.Entity<EmotionData>(entity =>
            {
                entity.HasKey(e => e.Time); // Assuming Time is unique and used as the primary key
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WinningEmotion).IsRequired();
                entity.Property(e => e.Seen).IsRequired();

            });
            modelBuilder.Entity<EnrollmentSummary>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Define composite key
                entity.HasKey(es => new { es.LessonId, es.StudentId });
                entity.Property(e => e.TeacherApproach)
                .HasMaxLength(1000)
                      .HasConversion(
                          v => string.Join(',', v),
                          v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                      .Metadata.SetValueComparer(
                          new ValueComparer<List<string>>(
                              (c1, c2) => c1.SequenceEqual(c2),
                              c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                              c => c.ToList()
                          ));


                // Remove Lesson navigation property from composite key
                // Configure LessonId as a foreign key instead
                entity.HasOne(es => es.Lesson)
                      .WithMany()
                      .HasForeignKey(es => es.LessonId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);



                // Configure StudentId as a foreign key
                entity.HasOne(es => es.Student)
                      .WithMany()
                      .HasForeignKey(es => es.StudentId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(es => es.EmotionData);
            });

        }
    }

    public static class DatabaseContextFactory
    {
        public static bool TestMode = true;
        public static DatabaseContext ConnectToDatabase()
        {
            return new DatabaseContext();
            
        }
    }
}
