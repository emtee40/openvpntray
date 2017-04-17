using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunTray
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start in background, if not running :-)
            if (Process.GetProcessesByName("OpenVPNTray").Length == 0)
                Process.Start("OpenVPNTray.exe");
        }
    }
}
