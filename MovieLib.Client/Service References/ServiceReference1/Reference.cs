﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieLib.Client.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IMovieService")]
    public interface IMovieService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMovieService/GetDirectorNames", ReplyAction="http://tempuri.org/IMovieService/GetDirectorNamesResponse")]
        MovieLib.Contracts.MovieData[] GetDirectorNames();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMovieService/GetDirectorNames", ReplyAction="http://tempuri.org/IMovieService/GetDirectorNamesResponse")]
        System.Threading.Tasks.Task<MovieLib.Contracts.MovieData[]> GetDirectorNamesAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMovieServiceChannel : MovieLib.Client.ServiceReference1.IMovieService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MovieServiceClient : System.ServiceModel.ClientBase<MovieLib.Client.ServiceReference1.IMovieService>, MovieLib.Client.ServiceReference1.IMovieService {
        
        public MovieServiceClient() {
        }
        
        public MovieServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MovieServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MovieServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MovieServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public MovieLib.Contracts.MovieData[] GetDirectorNames() {
            return base.Channel.GetDirectorNames();
        }
        
        public System.Threading.Tasks.Task<MovieLib.Contracts.MovieData[]> GetDirectorNamesAsync() {
            return base.Channel.GetDirectorNamesAsync();
        }
    }
}
