using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MovieLib.Core;
using MovieLib.Data.Entities;
using MovieLib.Data.SampleData;

namespace MovieLib.Data
{
   public class MovieReviewDbContext :DbContext
    {
       
       public MovieReviewDbContext() : base(nameOrConnectionString: "MoviesReviewProd") { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesReview> MovieReviews { get; set; }

        //invoke this to seed default values for the 1st run
        //comment the intializer code in production
        static MovieReviewDbContext()
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
           // Database.SetInitializer(new MovieReviewDatabaseInitializer());
            Database.SetInitializer<MovieReviewDbContext>(null);
        }

        //setting EF Convetions
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //use singular table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IIdentityEntity>();
            modelBuilder.Entity<Movie>().HasKey<int>(e => e.Id).Ignore(e => e.EntityId);
            modelBuilder.Entity<MoviesReview>().HasKey<int>(e => e.Id).Ignore(e => e.EntityId);
        }
    }
}
