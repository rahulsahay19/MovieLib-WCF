
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MovieLib.Core;
using MovieLib.Data.Entities;
using MovieLib.Data.Repository_Interfaces;


namespace MovieLib.Data.Repositories
{
    public class MovieRepository : DataRepositoryBase<Movie, MovieReviewDbContext>, IMovieRepository
    {
        protected override DbSet<Movie> DbSet(MovieReviewDbContext entityContext)
        {
            return entityContext.Movies;
        }

        protected override Expression<Func<Movie, bool>> IdentifierPredicate(MovieReviewDbContext entityContext, int id)
        {
            return (e => e.Id == id);
        }


        public IEnumerable<Movie> GetMovies()
        {
            using (MovieReviewDbContext entityContext = new MovieReviewDbContext())
            {
                return entityContext.Movies.ToList();

            }
        }
    }
}
