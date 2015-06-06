using System.ServiceModel;
using System.ServiceModel.Channels;
using MovieLib.Contracts;
using System.Collections.Generic;

namespace MovieLib.Proxies
{
   public class MovieClient : ClientBase<IMovieService>, IMovieService
   {
       public MovieClient(string endpointName):base(endpointName)
       {
           
       }

       public MovieClient(Binding binding,EndpointAddress address):base(binding,address)
       {
           
       }
       public IEnumerable<MovieData> GetDirectorNames()
       {

           return Channel.GetDirectorNames();
       }

       
       void IMovieService.UpdateMovieDirector(string moviename, string director)
       {
           throw new System.NotImplementedException();
       }
   }
}
