using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using MovieLib.Contracts;
using MovieLib.Services;

namespace ConsoleHostApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost hostMovieManager = new ServiceHost(typeof (MovieManager));
               
            hostMovieManager.Open();
            Console.WriteLine("Service Launched,Press Enter to Exit!");
            Console.ReadLine();
            hostMovieManager.Close();
        }
    }
}
