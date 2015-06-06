
using System.Collections.Generic;
using MovieLib.Core;
using MovieLib.Data.Entities;

namespace MovieLib.Data.Repository_Interfaces
{
    public interface IMovieRepository :IDataRepository<Movie>
    {
        IEnumerable<Movie> GetMovies();
    }
}
