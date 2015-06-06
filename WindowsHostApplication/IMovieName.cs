
using System.ServiceModel;

namespace WindowsHostApplication.Contracts
{
    [ServiceContract]
   public interface IMovieName
   {
        [OperationContract]
       void SelectedMovie(string moviename);
   }
}
