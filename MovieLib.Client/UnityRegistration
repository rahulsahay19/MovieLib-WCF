using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using Microsoft.Practices.Unity;

namespace Nasdaq.Corporate.Portal.IR.Api
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
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

            /*Working Piece inline*/
            /*container.RegisterType<IContactServiceAsync>(
                new TransientLifetimeManager(),
                new InjectionFactory(
                    (c) => new ChannelFactory<IContactServiceAsync>("IContactService").CreateChannel()));*/

            // LoadAssembly(container);
            CreateFactory(container);
        }


        private static void CreateFactory(IUnityContainer container)
        {
            Type t = null;
            Assembly assembly = Assembly.Load("Nasdaq.Corporate.Portal.IR.Data.Contract");
            foreach (Type ti in assembly.GetTypes().Where(x => x.IsInterface))
            {
                if (ti.GetCustomAttributes(true).OfType<ServiceContractAttribute>().Any())
                {
                    var incomingInterface = ti;

                    //For debugging Mapping Invoke that factory exclusively, as this won't step through from 
                    //Unity unless any exception occurs

                    // CreateGenericFactory(incomingInterface);

                    container.RegisterType(incomingInterface,
                       new TransientLifetimeManager(),
                       new InjectionFactory(
                         c => CreateGenericFactory(incomingInterface)
                       ));
                }
            }

        }

        private static object CreateGenericFactory(Type t)
        {

            Type factoryType = typeof(ChannelFactory<>);
            factoryType = factoryType.MakeGenericType(t);


            //Call the Binding section to get the associtaed binding
            var section = GetBindingsSection();
            Binding binding = null;
            string bindingName = null;

            List<Uri> addressCollection = new List<Uri>();
            List<string> contractCollection = new List<string>();
            List<string> bindinCollection = new List<string>();
            object address = null;

            ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            for (int j = 0; j < clientSection.Endpoints.Count; j++)
            {
                addressCollection.Add(clientSection.Endpoints[j].Address);
                contractCollection.Add(clientSection.Endpoints[j].Contract);
                bindinCollection.Add(clientSection.Endpoints[j].Binding);
            }


            for (int k = 0; k < addressCollection.Count; k++)
            {
                //Check for contract, if found, then pull corresponding address
                if (contractCollection[k].Contains(t.FullName))
                {
                    address = addressCollection[k].AbsoluteUri;
                    bindingName = clientSection.Endpoints[k].Binding;
                    //Now loop through collected binding name and check for configured bindings
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
                }
            }

            var factory = Activator.CreateInstance(factoryType, binding, address);

            MethodInfo method = factoryType.GetMethod("CreateChannel", new Type[] { });

            return method.Invoke(factory, null);
        }

        //This will return all the bindings from the config file
        private static BindingsSection GetBindingsSection()
        {
            BindingsSection bindingsSection = (BindingsSection)ConfigurationManager.GetSection("system.serviceModel/bindings");

            return bindingsSection;
        }

    }


}
