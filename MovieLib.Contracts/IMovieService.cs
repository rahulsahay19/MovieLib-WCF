using System;
using System.Collections.Generic;
using System.Net.Security;
using System.ServiceModel;

namespace MovieLib.Contracts
{
    [ServiceContract]
    public interface IMovieService
    {
        [OperationContract]
        [FaultContract(typeof(ApplicationException))]
        [FaultContract(typeof(DataNotFound))]
        IEnumerable<MovieData> GetDirectorNames();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateMovieDirector(string moviename, string director);
    }
}
