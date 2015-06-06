
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLib.Contracts;
using MovieLib.Services;

namespace MovieLib.Tests
{
    [TestClass]
    public class ServiceIntegrationTest
    {
        [TestMethod]
        public void Test_Movie_Name_Service()
        {
            string address = "net.pipe://localhost/MovieName";
            Binding binding = new NetNamedPipeBinding();

            ServiceHost host = new ServiceHost(typeof(MovieManager));

            host.AddServiceEndpoint(typeof (IMovieService), binding, address);

            host.Open();

            ChannelFactory<IMovieService> factory = new ChannelFactory<IMovieService>(binding,new EndpointAddress(address));

            IMovieService proxy = factory.CreateChannel();

            IEnumerable<MovieData> data = proxy.GetDirectorNames();

            Assert.IsTrue(data.Count().Equals(11));
        }
    }
}
