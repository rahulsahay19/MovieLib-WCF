using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MovieLib.Client.Contracts;
using MovieLib.Client.ServiceReference1;
using MovieLib.Contracts;
using MovieLib.Proxies;

namespace MovieLib.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private MovieClient proxyClient = null;
        private SynchronizationContext synchronizationContext = null;
        public MainWindow()
        {
            InitializeComponent();
            proxyClient = new MovieClient("1stEP");
           // proxyClient.ClientCredentials.UserName.UserName = "rahul";
            proxyClient.Open();
            synchronizationContext = SynchronizationContext.Current;
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
            try
            {
                await Task.Run(() =>
                {
                    IEnumerable<MovieData> data = proxyClient.GetDirectorNames();
                    if (data != null)
                    {
                        SendOrPostCallback callback = (arg =>
                        {
                            LstDirectors.ItemsSource = data;
                        });
                        synchronizationContext.Send(callback, true);
                    }

                    //   proxyClient.Close();
                });

            }
            catch (FaultException<ExceptionDetail> ex)
            {
                MessageBox.Show("Exception thrown by service.\n\r Exception Type:-" + ex +
                    "Message:-" + ex.Detail.Message + "\n\r" +
                    "Proxy State:- " + proxyClient.State);
            }

            catch (FaultException<ApplicationException> ex)
            {
                MessageBox.Show("Exception thrown by FaultException<ApplicationException>.\n\r Exception Type:-" + ex +
                    "Message:-" + ex.Detail.Message + "\n\r" +
                    "Proxy State:- " + proxyClient.State);
            }

            catch (FaultException<DataNotFound> ex)
            {
                MessageBox.Show("Exception thrown by FaultException<DataNotFound>.\n\r Exception Type:-" + ex +
                    "Message:-" + ex.Detail.Message + "\n\r" +
                    "User:- " + ex.Detail.User + "\n\r" +
                    "When:- " + ex.Detail.When + "\n\r" +
                    "Proxy State:- " + proxyClient.State);
            }

            catch (FaultException ex)
            {
                MessageBox.Show("Fault Exception thrown by service.\n\r Exception Type:-" + ex +
                    "Message:-" + ex.Message + "\n\r" +
                    "Proxy State:- " + proxyClient.State);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception thrown by service.\n\r Exception Type:-" + ex+
                    ex.GetType().Name+"\n\r"+
                    "Message:-" + ex.Message + "\n\r"+
                    "Proxy State:- " + proxyClient.State);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:8010/MovieService");
            Binding binding = new NetTcpBinding();

            MovieClient proxyClient = new MovieClient(binding, address);

            IEnumerable<MovieData> data = proxyClient.GetDirectorNames();

            if (data != null)
            {
                LstDirectors.ItemsSource = data;
            }

            proxyClient.Close();
        }

        private void btnInvoke_Click(object sender, RoutedEventArgs e)
        {
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:8011/MovieName");
            Binding binding = new NetTcpBinding();

            ChannelFactory<IMovieName> factory = new ChannelFactory<IMovieName>(binding, address);

            //ChannelFactory<IMovieName> factory = new ChannelFactory<IMovieName>("");

            IMovieName proxy = factory.CreateChannel();

            string value = txtMovieName.Text;
            proxy.ShowMovie(value);

            factory.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MovieServiceClient proxy = new MovieServiceClient();

            IEnumerable<MovieData> datas = proxy.GetDirectorNames();

            if (datas != null)
            {
                LstDirectors.ItemsSource = datas;
            }

            proxy.Close();

        }

    }
}

