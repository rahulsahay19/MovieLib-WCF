using System.ServiceModel;
using WindowsHostApplication.Contracts;

namespace WindowsHostApplication.Services
{
    [ServiceBehavior(UseSynchronizationContext = false,IncludeExceptionDetailInFaults = true)]
   public class MovieNameManager :IMovieName
    {
        public void SelectedMovie(string moviename)
        {
            MainWindow.PrimaryUI.SelectedMovie(moviename);
        }
    }
}
