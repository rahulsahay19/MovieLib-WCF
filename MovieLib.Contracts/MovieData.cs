
using System.Runtime.Serialization;

namespace MovieLib.Contracts
{
    [DataContract]
    public class MovieData
    {
        [DataMember]
        public string MovieName { get; set; }
        [DataMember]
        public string DirectorName { get; set; }
        [DataMember]
        public string ReleaseYear { get; set; }

    }
}
