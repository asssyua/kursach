using kursach.Model;
using System;
using System.Data.Entity;

namespace kursach.Database
{
    public class AppDbContext : DbContext
    {
      
        public DbSet<User> Users { get; set; }
        public DbSet<CardInformation> CardInformations { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; } 

        public DbSet<UserRequest> UserRequests { get; set; }

        public AppDbContext() : base("RealEstateConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .Property(u => u.login)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.passwordHash)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<CardInformation>()
                .ToTable("CardInformation")
                .HasKey(c => c.Id);

            modelBuilder.Entity<CardInformation>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<CardInformation>()
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<CardInformation>()
                .Property(c => c.ImageUrl)
                .HasMaxLength(500);

            modelBuilder.Entity<CardInformation>()
                .Property(c => c.Rental_Period)
                .HasMaxLength(50);

            modelBuilder.Entity<CardInformation>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);  

            modelBuilder.Entity<CardInformation>()
                .Property(c => c.Location)
                .HasMaxLength(255);

            modelBuilder.Entity<SliderImage>()
                .ToTable("SliderImages")
                .HasKey(s => s.Id);

            modelBuilder.Entity<SliderImage>()
                .Property(s => s.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<SliderImage>()
                .HasRequired(s => s.CardInformation) 
                .WithMany(c => c.SliderImages)  
                .HasForeignKey(s => s.CardInformationId)  
                .WillCascadeOnDelete(true);


            modelBuilder.Entity<UserRequest>()
                .ToTable("UserRequests")
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserRequest>()
                .Property(u => u.CardId)
                .IsRequired();

            modelBuilder.Entity<UserRequest>()
                .Property(u => u.UserId)
                .IsRequired();

            modelBuilder.Entity<UserRequest>()
                .Property(u => u.UserEmail)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<UserRequest>()
                .Property(u => u.UserFullName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<UserRequest>()
                .Property(u => u.UserBirthDate)
                .IsRequired();

            modelBuilder.Entity<UserRequest>()
                .Property(u => u.RentalTime)
                .HasMaxLength(50);

            modelBuilder.Entity<UserRequest>()
                .Property(u => u.Status)
                .IsRequired();

        }

    }
}
