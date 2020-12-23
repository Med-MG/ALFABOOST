using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ALFABOOST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private static double FullSize = 0;
        List<double>  DirTypesSizes = new List<double>();

        private void Close_Window(object sender, MouseButtonEventArgs e)
        {
            this.Close();
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

        //private void StartScan(object sender, MouseButtonEventArgs e)
        //{
        //    Analyzer StartScan = new Analyzer();
        //    ScanCircle.Value++;
        //}


        private void StartScan(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Analyze;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

      

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ScanCircle.Value = e.ProgressPercentage;
            WaveProgress.Value = e.ProgressPercentage;
        }


        

        void Analyze(object sender, DoWorkEventArgs e)
        {

            int count = 0;
            //var MyDiroctory = @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del";

            //Dictionary Of Diroctory Types,

            /*
             * A Dictionary Containing the type of Directories that we are going to analyze 
             * Type : Path
             * 
             */
            var DirTypesPaths = new Dictionary<string, string>()
            {
                {"Windows", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\windowsTemp" },
                {"Browsers", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\Browsers" },
                {"Software", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\Software" },
                {"Multimedia", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\Multimedia" },
            };



            try
            {

                foreach (var DirPaths in DirTypesPaths)
                {
                    double currentSize = 0;
                    
                    var dirInfo = new DirectoryInfo(DirPaths.Value);
                    foreach (FileInfo fi in dirInfo.GetFiles("*", SearchOption.AllDirectories))
                    {
                       
                        FullSize += fi.Length;
                        currentSize += fi.Length;

                        //Display Files paths in UI
                    }
                    (sender as BackgroundWorker).ReportProgress(count+=25);
                    Thread.Sleep(100);
                    DirTypesSizes.Add(currentSize);
                }

                DirTypesSizes.Add(FullSize);

                

            }
            catch (Exception)
            {

                throw;
            }

            DisplayCleaner();
            
        }

        void DisplayCleaner()
        {

            this.Dispatcher.Invoke(() =>
            {



                //Invoke Sotyboard Of cleaner
                Storyboard sb = this.FindResource("Select_Cleaner") as Storyboard;
                sb.Begin();
                //Hide The cards visibility
                LastScanCard.Visibility = Visibility.Hidden;
                LastCleanCard.Visibility = Visibility.Hidden;
                broom.Visibility = Visibility.Hidden;

                //Display result UI
                ResultOfScan.Visibility = Visibility.Visible;

                //Display result
                
                WinfilesSize.Text = SizeCalculator.ToFileSize(DirTypesSizes[0]);
                BrowserfilesSize.Text = SizeCalculator.ToFileSize(DirTypesSizes[1]);
                SoftwarefilesSize.Text = SizeCalculator.ToFileSize(DirTypesSizes[2]);
                MultimediafilesSize2.Text = SizeCalculator.ToFileSize(DirTypesSizes[3]);
                JunkCleaner.Text = SizeCalculator.ToFileSize(DirTypesSizes[4]);
            });

           
        }




        /*
         * History
         * Desentrelize Json File "hisroty" and display data
         * 
         */

        void DisplayHistory()
        {
            string historyData = File.ReadAllText("history.json");
            History hist = new History();

            Newtonsoft.Json.JsonConvert.PopulateObject(historyData, hist);
        }

    }
}
