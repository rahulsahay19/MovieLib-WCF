using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using WindowsHostApplication.Contracts;
using WindowsHostApplication.Services;
using MovieLib.Services;

namespace WindowsHostApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public static MainWindow PrimaryUI;
        public MainWindow()
        {
            InitializeComponent();
            Btnlaunch.IsEnabled = true;
            BtnStop.IsEnabled = false;

            PrimaryUI = this;
            this.Title = "UI Thread " + Thread.CurrentThread.ManagedThreadId +
               " & Process " + Process.GetCurrentProcess().Id.ToString();

            _synchronizationContext = SynchronizationContext.Current;
        }

        private ServiceHost _serviceHost = null;
        private ServiceHost _serviceHostMovieName = null;
        private SynchronizationContext _synchronizationContext = null;
        private void Btnlaunch_Click(object sender, RoutedEventArgs e)
        {
            _serviceHost = new ServiceHost(typeof(MovieManager));
            _serviceHostMovieName = new ServiceHost(typeof(MovieNameManager));

            _serviceHost.Open();
            _serviceHostMovieName.Open();
            Btnlaunch.IsEnabled = false;
            BtnStop.IsEnabled = true;

        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _serviceHost.Close();
            _serviceHostMovieName.Close();
            Btnlaunch.IsEnabled = true;
            BtnStop.IsEnabled = false;
        }

        public void SelectedMovie(string moviename)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            SendOrPostCallback sendOrPostCallback = (s =>
            {
                lblMovieName.Content =  moviename +Environment.NewLine+
                    "coming from thread: " +threadId+ " to thread: " +
                    Thread.CurrentThread.ManagedThreadId.ToString() +" Process: "+ Process.GetCurrentProcess().Id.ToString();
            });
            _synchronizationContext.Send(sendOrPostCallback, null);
        }

        private void BtnInProc_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                ChannelFactory<IMovieName> factory = new ChannelFactory<IMovieName>("");

                IMovieName proxy = factory.CreateChannel();


                proxy.SelectedMovie(
                    "Top Gun came at: " +
                        DateTime.Now);
                factory.Close();
            });

            thread.IsBackground = true;
            thread.Start();
        }
    }
}
