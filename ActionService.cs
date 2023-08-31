using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace WindownformInstallerService
{
    public static class ActionService
    {
        public static string connectionString = "Data Source=ServiceManagement.db;Version=3;";
        public static bool CheckServiceExists(string name)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckServiceExistsInListManager(string name)
        {
            string selectDataQuery = "SELECT * FROM Services";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(selectDataQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(2) == name)
                            {
                                return true;
                            }
                        }
                    }
                }
                connection.Close();
            }
            return false;
        }
        public static bool CheckStatusService(string name)
        {
            using (ServiceController serviceController = new ServiceController(name))
            {
                if (serviceController.Status == ServiceControllerStatus.Running ||
                            serviceController.Status == ServiceControllerStatus.StartPending)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static string GetDescriptionService(string name)
        {
            using (ServiceController serviceController = new ServiceController(name))
            {
                return serviceController.DisplayName;
            }
        }
        public static string CheckDetailStatusService(string name)
        {
            using (ServiceController serviceController = new ServiceController(name))
            {
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    return "Stopped";
                }
                if (serviceController.Status == ServiceControllerStatus.StartPending)
                {
                    return "StartPending";
                }
                if (serviceController.Status == ServiceControllerStatus.StopPending)
                {
                    return "StopPending";
                }
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    return "Running";
                }
                if (serviceController.Status == ServiceControllerStatus.ContinuePending)
                {
                    return "ContinuePending";
                }
                if (serviceController.Status == ServiceControllerStatus.PausePending)
                {
                    return "PausePending";
                }
                if (serviceController.Status == ServiceControllerStatus.Paused)
                {
                    return "Paused";
                }
            }
            return "";
        }
        public static void StartService(string name)
        {
            try
            {
                using (ServiceController serviceController = new ServiceController(name))
                {
                    if (serviceController.Status == ServiceControllerStatus.Stopped)
                    {
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                    }
                }
                MessageBox.Show("Service is running", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void RestartService(string name)
        {
            try
            {
                using (ServiceController serviceController = new ServiceController(name))
                {
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    }
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                    MessageBox.Show("Service has restarted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void StopService(string name)
        {
            try
            {
                using (ServiceController serviceController = new ServiceController(name))
                {
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    }
                }
                MessageBox.Show("Service has stopped", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void InstallerService(string patch)
        {
            string[] exeFiles = Directory.GetFiles(patch, "*.exe");
            string parameters = "i";
            Process.Start(exeFiles[0], parameters);
        }
        public static void UninstallerService(string patch)
        {
            string[] exeFiles = Directory.GetFiles(patch, "*.exe");
            string parameters = "u";
            Process.Start(exeFiles[0], parameters);
        }

    }
}
