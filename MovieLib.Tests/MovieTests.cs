using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieLib.Contracts;
using MovieLib.Data.Entities;
using MovieLib.Data.Repository_Interfaces;
using MovieLib.Services;

namespace MovieLib.Tests
{
    [TestClass]
    public class MovieTests
    {
        private static Mock<IMovieRepository> MovieRepository(out IEnumerable<Movie> movie)
        {
            Mock<IMovieRepository> mockMovieRepository = new Mock<IMovieRepository>();

            //Mock return as GetMovies returns the same, so we are not going to hit db
            //we are going to return mocked up entity
            movie = new Movie[]
            {
                new Movie()
                {
                    MovieName = "Avatar",
                    DirectorName = "James Cameron",
                    ReleaseYear = "2009"
                },
                new Movie()
                {
                    MovieName = "Titanic",
                    DirectorName = "James Cameron",
                    ReleaseYear = "1997"
                }
            };
            return mockMovieRepository;
        }
        [TestMethod]
        public void MovieTest()
        {
            IEnumerable<Movie> movie;
            var mockMovieRepository = MovieRepository(out movie);

            //so, now i am going to setup the mock, Hence below i am telling that when you are encountering
            //following member of mockMovieRepository that receives the following information of GetMovies
            //obj is the implementation of mockMovieRepository. see, mock is creating the test class behind 
            //the scene.
            mockMovieRepository.Setup(obj => obj.GetMovies()).Returns(movie);

            IMovieService movieService = new MovieManager(mockMovieRepository.Object);

            IEnumerable<MovieData> data = movieService.GetDirectorNames();

            Assert.AreEqual(2,data.Count());
           // Assert.IsTrue(data.GetEnumerator().Current.DirectorName);

            //Assert.IsTrue(data.ElementAt(0)("James Cameron"));
        }

        [TestMethod]
        public void MovieNameTest()
        {
            IEnumerable<Movie> movie;
            var mockMovieRepository = MovieRepository(out movie);

            mockMovieRepository.Setup(obj => obj.GetMovies()).Returns(movie);

            IMovieService movieService = new MovieManager(mockMovieRepository.Object);

            IEnumerable<MovieData> data = movieService.GetDirectorNames();

            data.Should().HaveCount(4, "because we put these many values only");
            
            
        }

    }
}
