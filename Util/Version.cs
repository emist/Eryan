using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Util
{
    static class Version
    {
        public enum WindowsVersions { UnKnown,
        Win95, Win98, WinMe, WinNT3or4, Win2000, WinXP, WinServer2003, WinVista,
        Win7, MacOSX, Unix,
        Xbox };

        public static
        WindowsVersions GetCurrentWindowsVersion()

        {

        // Get OperatingSystem information from the system namespace.

        System.OperatingSystem osInfo = System.Environment.OSVersion;

         

        // Determine the platform.

        if (osInfo.Platform == System.PlatformID.Win32Windows)
        {

        // Platform is Windows 95, Windows 98, Windows 98 Second Edition, or Windows Me.

        switch (osInfo.Version.Minor)

        {

        case 0:


        //Console.WriteLine("Windows 95");


        return WindowsVersions.Win95;

         

        case 10:

        //if (osInfo.Version.Revision.ToString() == "2222A")


        // Console.WriteLine("Windows 98 Second Edition");


        //else


        // Console.WriteLine("Windows 98");


        return WindowsVersions.Win98;

         

        case 90:


        //Console.WriteLine("Windows Me");


        return WindowsVersions.WinMe;

        }

        }

        else if (osInfo.Platform == System.PlatformID.Win32NT)

        {

        // Platform is Windows NT 3.51, Windows NT 4.0, Windows 2000, or Windows XP.

        switch (osInfo.Version.Major)

        {

        case 3:

        case 4:


        //Console.WriteLine("Windows NT 3.51"); // = 3


        //Console.WriteLine("Windows NT 4.0"); // = 4


        return WindowsVersions.WinNT3or4;

         

        case 5:


        switch (osInfo.Version.Minor)


        {


        case 0:

        //name = "Windows 2000";

        return WindowsVersions.Win2000;


        case 1:

        //name = "Windows XP";

        return WindowsVersions.WinXP;


        case 2:

        //name = "Windows Server 2003";

        return WindowsVersions.WinServer2003;


        }


        break;

         

        case 6:


        switch (osInfo.Version.Minor)


        {


        case 0:

        // Windows Vista or Windows Server 2008 (distinct by rpoduct type)

        return WindowsVersions.WinVista;

         


        case 1:

        return WindowsVersions.Win7;


        }


        break;

        }

        }

        else if (osInfo.Platform == System.PlatformID.Unix)

        {

        return WindowsVersions.Unix;

        }

        else if (osInfo.Platform == System.PlatformID.MacOSX)

        {

        return WindowsVersions.MacOSX;

        }

        else if (osInfo.Platform == PlatformID.Xbox)

        {

        return WindowsVersions.Xbox;

        }

        return WindowsVersions.UnKnown;

        }
    }
}
