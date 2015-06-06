
using System.ServiceModel;

namespace MovieLib.Client.Contracts 
{
    [ServiceContract]
   public interface IMovieName
   {
        [OperationContract(Name = "SelectedMovie")]
       void ShowMovie(string moviename);
   }
}
