
using System.Runtime.Serialization;

namespace MovieLib.Contracts
{
    [DataContract]
   public class DataNotFound
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string When { get; set; }

        [DataMember]
        public string User { get; set; }

    }
}
