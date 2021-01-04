using ChaseLabs.CLUpdate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPDATER
{
    public class UpdateChecker
    {
        string url = "https://www.dropbox.com/s/r1jy5d3rqw2n0nz/ALFABOOST.zip?dl=1";
        string version_key = "application: ";
        string remote_version_url = "https://www.dropbox.com/s/g5edeto5w14edzd/version.txt?dl=1";
        string update_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ALFABOOST", "AutoUpdater", "Update");
        string application_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ALFABOOST", "AutoUpdater", "bin");
        string local_version_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ALFABOOST", "AutoUpdater", "Version");
        string launch_exe = "UPDATER.exe";

        public bool UpdateCheck()
        {
            UpdateManager upManager = new UpdateManager();
            if (upManager.CheckForUpdate(version_key, local_version_path, remote_version_url))
            {
                return true;
            }
            return false;

        }

        public void LaunchUpdater()
        {
            Process.Start(launch_exe);
        }
    }
}
