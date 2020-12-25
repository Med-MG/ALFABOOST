using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ChaseLabs.CLUpdate;
namespace UPDATER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Dispatcher dis = Dispatcher.CurrentDispatcher;
        public MainWindow()
        {
            InitializeComponent();
            Update();
        }

        private void Minimize_Window(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void max_window(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
        private void Drage_appRight(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        void Update()
        {
            Task.Run(() =>
           {

               dis.Invoke(() =>
              {
                ButtonT.Content = "Checking for Updates";

              }, DispatcherPriority.Normal);

               
               string url = "https://www.dropbox.com/s/r1jy5d3rqw2n0nz/ALFABOOST.zip?dl=1";
               string version_key = "application: ";
               string remote_version_url = "https://www.dropbox.com/s/r1jy5d3rqw2n0nz/ALFABOOST.zip?dl=1";
               string update_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ALFABOOST", "AutoUpdater", "Update");
               string application_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ALFABOOST", "AutoUpdater", "bin");
               string local_version_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ALFABOOST", "AutoUpdater", "Version");
               string launch_exe = "ALFABOOST.exe";

               var update = Updater.Init(url, update_path, application_path, launch_exe);
               UpdateManager upManager = new UpdateManager();
               if (upManager.CheckForUpdate(version_key, local_version_path, remote_version_url))
               {
                   dis.Invoke(() =>
                   {
                       ButtonT.Content = "Update Found";

                   }, DispatcherPriority.Normal);

                   update.Download();
                   dis.Invoke(() =>
                   {
                       ButtonT.Content = "Downloading update";

                   }, DispatcherPriority.Normal);

                   update.Unzip();
                   dis.Invoke(() =>
                   {
                       ButtonT.Content = "Unziping Update...";

                   }, DispatcherPriority.Normal);

                   update.CleanUp();

                   dis.Invoke(() =>
                   {
                       ButtonT.Content = "Finishing up...";

                   }, DispatcherPriority.Normal);


                   using (var client = new System.Net.WebClient())
                   {
                       client.DownloadFile(remote_version_url, local_version_path);
                       client.Dispose();
                   }


               }

               dis.Invoke(() =>
               {
                   ButtonT.Content = "Launch Application";

               }, DispatcherPriority.Normal);

               update.LaunchExecutable();
           });
        }
    }
}
