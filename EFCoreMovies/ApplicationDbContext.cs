﻿using EFCoreMovies.Entities;
using EFCoreMovies.Entities.Configurations;
using EFCoreMovies.Entities.Functions;
using EFCoreMovies.Entities.Keyless;
using EFCoreMovies.Entities.Seeding;
using EFCoreMovies.Utilities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Operation.Overlay.Validate;
using System.Reflection;
using System.Text;

namespace EFCoreMovies
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions options): base(options) 
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
            configurationBuilder.Properties<string>().HaveMaxLength(150);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            Module3Seeding.Seed(modelBuilder);
            Module6Seeding.Seed(modelBuilder);
            Module9Seeding.Seed(modelBuilder);

            SomeConfiguration(modelBuilder);

            Scalars.RegisterFunctions(modelBuilder);
        }

        //This method is just a placeholder DbFunction attribute indicates that a database function should be triggered
        [DbFunction]
        public int InvoiceDetailSum(int invoiceId)
        {
            return 0;
        }

        public IQueryable<MovieWithCounts> MoviesWithCounts(int movieId)
        {
            return FromExpression(() => MoviesWithCounts(movieId));
        }

        private void SomeConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CinemaWithoutLocation>().ToSqlQuery("SELECT Id, Name FROM Cinemas").ToView(null);
            //modelBuilder.Entity<MovieWithCounts>().ToView("MoviesWithCounts");

            modelBuilder.Entity<MovieWithCounts>().HasNoKey().ToTable(name: null);

            modelBuilder.HasDbFunction(() => MoviesWithCounts(0)).HasName("MovieWithCounts");
            
            modelBuilder.Entity<RentableMovie>().ToTable("RentableMovies");
            modelBuilder.Entity<Merchandising>().ToTable("Merchandising");

            var movie1 = new RentableMovie()
            {
                Id = 1,
                Name = "Spider-Man",
                MovieId = 1,
                Price = 5.99m
            };

            var merch1 = new Merchandising()
            {
                Id = 2,
                Available = true,
                IsClothing = true,
                Name = "One Piece T-Shirt",
                Weight = 1,
                Volume = 1,
                Price = 11
            };

            modelBuilder.Entity<Merchandising>().HasData(merch1);
            modelBuilder.Entity<RentableMovie>().HasData(movie1);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaOffer> CinemaOffers { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<MovieActor> MoviesActors { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<CinemaWithoutLocation> CinemasWithoutLocations { get; set; }
        public DbSet<Person> People { get; set; }   
        public DbSet<Message> Messages { get; set; }
        public DbSet<CinemaDetail> CinemaDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
