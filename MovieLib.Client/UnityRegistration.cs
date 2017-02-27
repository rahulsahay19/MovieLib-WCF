using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;
using MovieLib.Corporate.Core.Service;
using MovieLib.Corporate.Core.Service.Configuration;
using MovieLib.Corporate.Core.Wcf.Behaviours;
using MovieLib.Portal.Shell.Infrastructure.Common.Context.Wcf;

namespace MovieLib.Portal.Shell.Api
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        /// <summary>
        /// Configuration section name.
        /// </summary>
        private const string ConfigurationSection = "serviceConfiguration";

        private const string ChannelWcf = "wcf";
        private const string ChannelInproc = "inproc";

        /// <summary>
        /// List of all available in-proc services
        /// </summary>
        private static Dictionary<string, IService> inProcServices = new Dictionary<string, IService>();

        /// <summary>
        /// List of all available wcf services
        /// </summary>
        private static HashSet<string> wcfServices = new HashSet<string>();

        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            //container.LoadConfiguration();

            // TODO: Register your types here

            //for in proc
            // container.RegisterType<IMarketDataServiceAsync>().
            /*Working Piece inline*/
            /*container.RegisterType<IContactServiceAsync>(
                new TransientLifetimeManager(),
                new InjectionFactory(
                    (c) => new ChannelFactory<IContactServiceAsync>("IContactService").CreateChannel()));*/

            // LoadAssembly(container);

            // NavigationLayouts.ResolveProperty = () => container.Resolve<INavigationServiceAsync>();
            /*container.RegisterType<ShellContextHeaderClientBehavior>();*/

            CreateFactory(container);
        }

        private static void CreateFactory(IUnityContainer container)
        {
            //For inproc, Below method is just there to isolate the inproc stuff from wcf registration
            //In-Proc Calls shouldn't be part of Unity Registration.
            CheckInProc();

            var clientSection = GetClientSection();

            //Logic to read the endpoints and construct assembly on the fly
            for (int i = 0; i < clientSection.Endpoints.Count; i++)
            {
                var contract = clientSection.Endpoints[i].Contract;
                var typeParts = contract.Split('.');
                var constructAssembly = "";
                var endPointName = clientSection.Endpoints[i].Name;

                //This logic is there to skip N3A API, as this won't be needed for registration process.
                if (endPointName != "IAuthenticationSessionApi")
                {
                    for (int j = 0; j < typeParts.Length - 1; j++)
                    {
                        constructAssembly += typeParts[j] + ".".Trim();
                    }
                    constructAssembly = constructAssembly.TrimEnd('.');
                }

               
                Assembly inassembly = null;
                if (constructAssembly != "")
                {
                    /*TODO:- Below stuff is temprorary*/
                    if (constructAssembly != "RequestReduce.SqlServer.Data.Service.Contract")
                    {
                        inassembly = Assembly.Load(constructAssembly);
                    }
                }
                if (inassembly != null)
                    foreach (Type ti in inassembly.GetTypes().Where(x => x.IsInterface))
                    {
                        if (ti.GetCustomAttributes(true).OfType<ServiceContractAttribute>().Any())
                        {
                            var incomingInterface = ti;

                            //For debugging Mapping Invoke that factory exclusively, as this won't step through from 
                            //Unity unless any exception occurs

                            //  CreateGenericFactory(incomingInterface);

                            container.RegisterType(incomingInterface,
                                new TransientLifetimeManager(),
                                new InjectionFactory(
                                    c => CreateGenericFactory(incomingInterface)
                                    ));
                        }
                    }
            }

        }

        //Method to check whether in proc or not. 
        //TODO:- This will actually come in picture in future, when In-Procs services will be there.
        private static bool CheckInProc()
        {
            var configSection = ConfigurationManager.GetSection(ConfigurationSection) as ServiceConfigurationSection;
            bool inProcFlag = false;
            if (configSection != null)
            {
                var svcConfig = configSection.Services;
                foreach (ServiceConfigurationElement config in svcConfig)
                {
                    if (config.IsEnabled)
                    {
                        if (config.Channel == ChannelInproc)
                        {
                            //If channel is inproc, load the assembly, create the instance and add to the list for caching.
                            var typeParts = config.Type.Split(',');
                            var assemblyName = typeParts[1].Trim();
                            var inassembly = Assembly.Load(assemblyName);
                            var service = inassembly.CreateInstance(typeParts[0].Trim()) as IService;

                            inProcFlag = true;
                        }
                    }
                }
            }
            return inProcFlag;
        }

        private static object CreateGenericFactory(Type t)
        {

            Type factoryType = typeof(ChannelFactory<>);
            factoryType = factoryType.MakeGenericType(t);

            //Call the Binding section to get the associtaed binding
            var section = GetBindingsSection();

            Binding binding = null;
            string bindingName = null;

            List<string> contractCollection;
            object address;
            ClientSection clientSection;

            var addressCollection = ApplyAddressCollection(out contractCollection, out address, out clientSection);

            //Loop through the address collection and check for required contract
            for (int k = 0; k < addressCollection.Count; k++)
            {
                //Check for contract, if found, then pull corresponding address
                if (contractCollection[k].Contains(t.FullName))
                {
                    address = addressCollection[k].AbsoluteUri;
                    bindingName = clientSection.Endpoints[k].Binding;
                    //Now loop through collected binding name and check for configured bindings
                    binding = ApplyBindingConfiguration(section, bindingName, binding);
                }
            }

            var factory = Activator.CreateInstance(factoryType, binding, address);

            ApplyEndPointBehaviors(factory);

            MethodInfo method = factoryType.GetMethod("CreateChannel", new Type[] { });

            return method.Invoke(factory, null);
        }

        //Method to Apply Endpoint behaviors 
        private static void ApplyEndPointBehaviors(object factory)
        {
            ((ChannelFactory)factory).Endpoint.Behaviors.Add(new PortalContextHeaderClientBehavior());
            ((ChannelFactory)factory).Endpoint.Behaviors.Add(new CorpFaultBehavior());
        }

        //Method to collect all EndPoint addresses
        private static List<Uri> ApplyAddressCollection(out List<string> contractCollection, out object address,
            out ClientSection clientSection)
        {
            List<Uri> addressCollection = new List<Uri>();
            contractCollection = new List<string>();
            List<string> bindinCollection = new List<string>();
            address = null;

            clientSection = GetClientSection();

            for (int j = 0; j < clientSection.Endpoints.Count; j++)
            {
                addressCollection.Add(clientSection.Endpoints[j].Address);
                contractCollection.Add(clientSection.Endpoints[j].Contract);
                bindinCollection.Add(clientSection.Endpoints[j].Binding);
            }
            return addressCollection;
        }

        //Method to apply the Binding Collection
        private static Binding ApplyBindingConfiguration(BindingsSection section, string bindingName, Binding binding)
        {
            foreach (var bindingCollection in section.BindingCollections)
            {
                if (bindingCollection.BindingName.Equals(bindingName))
                {
                    for (int i = 0; i < bindingCollection.ConfiguredBindings.Count; i++)
                    {
                        if (bindingCollection.ConfiguredBindings.Count > 0)
                        {
                            var bindingElement = bindingCollection.ConfiguredBindings[i];
                            binding = (Binding)Activator.CreateInstance(bindingCollection.BindingType);
                            binding.Name = bindingElement.Name;
                            bindingElement.ApplyConfiguration(binding);
                        }
                    }
                }
            }
            return binding;
        }

        //Method to fetch Client Section
        private static ClientSection GetClientSection()
        {
            ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            return clientSection;

        }


        //This will return all the bindings from the config file
        private static BindingsSection GetBindingsSection()
        {
            BindingsSection bindingsSection = (BindingsSection)ConfigurationManager.GetSection("system.serviceModel/bindings");

            return bindingsSection;
        }

    }
}
