using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace MovieLib.Contracts
{
    [DataContract]
   public class IMovieDirector
    {
        [DataMember]
        public string MovieName { get; set; }
        [DataMember]
        public string DirectorName { get; set; }
    }
}
