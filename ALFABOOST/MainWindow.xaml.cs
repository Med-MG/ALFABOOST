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


        private static double FullSize = 0;
        List<double> DirTypesSizes = new List<double>();
        bool _ModeClean = false;
        IDictionary<string, string> DirTypesPaths = new Dictionary<string, string>()
            {
                {"Windows", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\windowsTemp" },
                {"Browsers", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\Browsers" },
                {"Software", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\Software" },
                {"Multimedia", @"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\Multimedia" },
            };

        public MainWindow()
        {
            InitializeComponent();
        }


        



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

        


        private void StartScan(object sender, RoutedEventArgs e)
        {
            _ModeClean = true;
            Add_Current_Scan_Date();
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
                       
                        

                    }
                    (sender as BackgroundWorker).ReportProgress(count += 25);
                    Thread.Sleep(3500);

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



                ////Invoke Sotyboard Of cleaner
                Storyboard sb = this.FindResource("Select_Cleaner") as Storyboard;
                sb.Begin();

                Cleaner_Layout_Display();

                //Display result
                int FilesCount = Directory.GetFiles(@"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\", "*", SearchOption.AllDirectories).Length;

                WinfilesSize.Text = SizeCalculator.ToFileSize(DirTypesSizes[0]);
                BrowserfilesSize.Text = SizeCalculator.ToFileSize(DirTypesSizes[1]);
                SoftwarefilesSize.Text = SizeCalculator.ToFileSize(DirTypesSizes[2]);
                MultimediafilesSize2.Text = SizeCalculator.ToFileSize(DirTypesSizes[3]);
                ResutFullSize.Text = SizeCalculator.ToFileSize(DirTypesSizes[4]);
                subSize.Text = Convert.ToString(FilesCount);
            });

           
        }





        /*
         *Method to get Current SCan date
         *
         */
        void Add_Current_Scan_Date()
        {
            var hist = populate_History_Object();
            DateTime now = DateTime.Now;
            hist.lastScan = Convert.ToString(now);
            hist.numOfScans = Convert.ToString(Convert.ToInt32(hist.numOfScans) + 1);
            string newHistory = Newtonsoft.Json.JsonConvert.SerializeObject(hist);
            File.WriteAllText(@"C:\Users\Administrateur\source\repos\ALFABOOST\ALFABOOST\History.json", newHistory);
            
        }

        /*
         * History
         * Desentrelize Json File "hisroty" and display data
         * 
         */

        private void Display_History(object sender, EventArgs e)
        {

            var hist = populate_History_Object();
            
            LastScanDate1.Text = hist.ToTimeSinceString(Convert.ToDateTime(hist.lastScan));
            LastCleanDate1.Text = hist.ToTimeSinceString(Convert.ToDateTime(hist.lastClean));
            GainedSize.Text = hist.gained;
            NumScanHist1.Text = hist.numOfScans;
            NumCleanHist1.Text = hist.numOfClean;
        }

        public History populate_History_Object()
        {
            History hist = new History();
            string historyData = File.ReadAllText(@"C:\Users\Administrateur\source\repos\ALFABOOST\ALFABOOST\History.json");
            Newtonsoft.Json.JsonConvert.PopulateObject(historyData, hist);
            return hist;
        }


        /*
         * Click Cleaner Button to toggle Layouts
         * And dispalay Last Clean And Last Scan
         */

        private void Cleaner_Click(object sender, RoutedEventArgs e)
        {
            var hist = populate_History_Object();
            LastScanDate.Text = hist.ToTimeSinceString(Convert.ToDateTime(hist.lastScan));
            LastCleanDate.Text = hist.ToTimeSinceString(Convert.ToDateTime(hist.lastClean));


            Cleaner_Layout_Display();

        }

        void Cleaner_Layout_Display()
        {
            if (_ModeClean)
            {
                this.Dispatcher.Invoke(() =>
                {
                    //Invoke Sotyboard Of cleaner
                    Storyboard sbClean = this.FindResource("DisplayClean") as Storyboard;
                    sbClean.Begin();
                });
            }
            else if (!_ModeClean)
            {
                this.Dispatcher.Invoke(() =>
                { 
                //Invoke Sotyboard Of cleaner
                    Storyboard sbScan = this.FindResource("DisplaySCANL") as Storyboard;
                    sbScan.Begin();
                });
                    
            }
        }




        /*
         * Cleaner Related Functions .
         * Clean data, Display Progress bar
         * Reset Software 
         * 
        */

        private void StartClean(object sender, RoutedEventArgs e)
        {
            _ModeClean = false;

            
            ////Invoke Sotyboard Of cleaner
            Storyboard sbCleanProg = this.FindResource("CleanerProgressStart") as Storyboard;
            sbCleanProg.Begin();

            Add_Current_Clean_History();
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Clean;
            worker.ProgressChanged += Cleaner_ProgressChanged;

            worker.RunWorkerAsync();
        }

        void Cleaner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CleanerProgress.Value = e.ProgressPercentage;
           
        }

        void Clean(object sender, DoWorkEventArgs e)
        {
            //int count = 0;
            var MaindirInfo = new DirectoryInfo(@"C:\Users\Administrateur\Downloads\Production\YoucodeC#\WPF\FilesToClean\del\");
            //int FilesCount = MaindirInfo.GetFiles("*", SearchOption.AllDirectories).Length;
            foreach (FileInfo fi in MaindirInfo.GetFiles("*", SearchOption.AllDirectories))
            {

                //fi.Delete();
                //(sender as BackgroundWorker).ReportProgress(count += (100/ FilesCount));
                //Thread.Sleep(100);


            }
            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }


            this.Dispatcher.Invoke(() =>
            {
                //Invoke Sotyboard Of cleaner
                Storyboard CleanDone = this.FindResource("CloseCleanerProgress") as Storyboard;
                CleanDone.Begin();
                //Reset Results
                WinfilesSize.Text = "0.0 MB";
                BrowserfilesSize.Text = "0.0 MB";
                SoftwarefilesSize.Text = "0.0 MB";
                MultimediafilesSize2.Text = "0.0 MB";
                ResutFullSize.Text = "0.0 GB";
                subSize.Text = "0";
                WaveProgress.Value = 0;
                ScanCircle.Value = 0;
            });

        }

        

        void Add_Current_Clean_History()
        {
            var hist = populate_History_Object();
            DateTime now = DateTime.Now;
            hist.lastClean = Convert.ToString(now);
            hist.numOfClean = Convert.ToString(Convert.ToInt32(hist.numOfClean ) + 1);
            hist.gained = Convert.ToString(SizeCalculator.ToFileSize(FullSize));
            string newHistory = Newtonsoft.Json.JsonConvert.SerializeObject(hist);
            File.WriteAllText(@"C:\Users\Administrateur\source\repos\ALFABOOST\ALFABOOST\History.json", newHistory);

        }

    }
}
