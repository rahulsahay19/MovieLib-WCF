
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using MovieLib.Contracts;
using MovieLib.Data.Entities;
using MovieLib.Data.Repositories;
using MovieLib.Data.Repository_Interfaces;

namespace MovieLib.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true)]
    public class MovieManager : IMovieService
    {
        private IMovieRepository _iMovieRepository;
        private int _counter = 0;
        public MovieManager()
        {

        }

        public MovieManager(IMovieRepository iMovieRepository)
        {
            _iMovieRepository = iMovieRepository;
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
        public IEnumerable<MovieData> GetDirectorNames()
        {
            
            string hostIdentity = WindowsIdentity.GetCurrent().Name;
           // string primaryIdentity = ServiceSecurityContext.Current.PrimaryIdentity.Name;
           // string windowsIdentity = ServiceSecurityContext.Current.WindowsIdentity.Name;
           // string threadIdentity = Thread.CurrentPrincipal.Identity.Name;
           
            List<MovieData> movieData = new List<MovieData>();

            
                IMovieRepository movieRepository = _iMovieRepository ?? new MovieRepository();

                IEnumerable<Movie> movies = movieRepository.GetMovies();

            if (movies.Count() != 0)
            {
                foreach (Movie movie in movies)
                {
                    movieData.Add(new MovieData()
                    {
                        DirectorName = movie.DirectorName
                    });
                }
            }
            else
            {
                //throw new Exception("List is empty");
               // throw new FaultException("List is empty");
                //ApplicationException ex = new ApplicationException("List is empty");

                //throw new FaultException<ApplicationException>(ex,"Wrong DB instance");

                DataNotFound ex = new DataNotFound()
                {
                    Message = "Data Not Found",
                    When = DateTime.Now.ToString(),
                    User = "Rahul"
                };
                throw new FaultException<DataNotFound>(ex,"Custom Exception");
            }
                _counter++;
                //Console.WriteLine("Counter:- {0}",_counter);
                MessageBox.Show("Counter:-" +_counter);
            
               
           
            return movieData;
        }

        
        void IMovieService.UpdateMovieDirector(string moviename, string director)
        {
            throw new NotImplementedException();
        }
    }
}
