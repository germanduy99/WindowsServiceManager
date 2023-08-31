using System;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.ServiceProcess;


namespace MyService1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            bool isServiceInstalled = IsServiceInstalled("DuyService1");

            if (Environment.UserInteractive)
            {
                string firstArg = args[0];
                if (firstArg == "i")
                {
                    if (!isServiceInstalled)
                    {
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                    }
                }
                if (firstArg == "u")
                {
                    if (isServiceInstalled)
                    {
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                    }
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service1()
                };
                ServiceBase.Run(ServicesToRun);

            }
        }

        static bool IsServiceInstalled(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
