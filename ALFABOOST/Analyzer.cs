using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ALFABOOST
{
    class Analyzer 
    {
        private static long FullSize = 0;

        public IDictionary<string, long> Analyze()
        {

            IDictionary<string, long> DirTypesSizes = new Dictionary<string, long>();
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

                foreach(var DirPaths in DirTypesPaths)
                {
                    DirTypesSizes.Add(DirPaths.Key, GetSize(DirPaths.Value));
                }

                DirTypesSizes.Add("FullSize", FullSize);


            }
            catch (Exception)
            {

                throw;
            }


            return DirTypesSizes;
        }

        public static long GetSize(string path)
        {
            long currentSize = 0;
            var dirInfo = new DirectoryInfo(path);
            foreach (FileInfo fi in dirInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                FullSize += fi.Length;
                currentSize += fi.Length;
                
                //Display Files paths in UI

            }

            return currentSize;
        }
    }
}
