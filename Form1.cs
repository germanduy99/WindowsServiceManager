using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;

namespace WindownformInstallerService
{
    public partial class Form1 : Form
    {
        private DetailService detailService;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            initGridView();
            listToGridView();
            dataGridView1.CellContentClick += dataGridView1_OnClick;
            dataGridView1.CellMouseClick += dataGridView1_DeleteService;
        }
        private void initGridView()
        {
            DataGridViewCheckBoxColumn install = new DataGridViewCheckBoxColumn();
            install.HeaderText = "Install";
            install.Name = "install";
            dataGridView1.Columns.Add(install);
            dataGridView1.Columns["install"].Width = 50;

            DataGridViewTextBoxColumn nameService = new DataGridViewTextBoxColumn();
            nameService.HeaderText = "Service";
            nameService.Name = "nameService";
            nameService.ReadOnly = true;
            dataGridView1.Columns.Add(nameService);
            dataGridView1.Columns["nameService"].Width = 120;

            DataGridViewTextBoxColumn description = new DataGridViewTextBoxColumn();
            description.HeaderText = "Description";
            description.Name = "description";
            description.ReadOnly = true;
            dataGridView1.Columns.Add(description);
            dataGridView1.Columns["description"].Width = 250;

            DataGridViewImageColumn status = new DataGridViewImageColumn();
            status.HeaderText = "Status";
            status.Name = "status";
            dataGridView1.Columns.Add(status);
            dataGridView1.Columns["status"].Width = 50;

            DataGridViewTextBoxColumn path = new DataGridViewTextBoxColumn();
            path.HeaderText = "Path";
            path.Name = "path";
            path.ReadOnly = true;
            dataGridView1.Columns.Add(path);
            dataGridView1.Columns["path"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = false;

        }
        public void listToGridView()
        {
            List<string> listName = new List<string>();
            string selectDataQuery = "SELECT * FROM Services";
            using (SQLiteConnection connection = new SQLiteConnection(ActionService.connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(selectDataQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listName.Add(reader.GetString(2));
                        }
                    }
                }
                connection.Close();
            }
            listName.ForEach(value => CheckDetailService(value));
            dataGridView1.Rows.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(ActionService.connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(selectDataQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetInt32(1) == 1)
                            {
                                if (ActionService.CheckStatusService(reader.GetString(2)))
                                {
                                    dataGridView1.Rows.Add(reader.GetInt32(1), reader.GetString(2), reader.GetString(3), ActionImage.SetImage(ActionImage.iconStop), reader.GetString(5));
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(reader.GetInt32(1), reader.GetString(2), reader.GetString(3), ActionImage.SetImage(ActionImage.iconPlay), reader.GetString(5));
                                }
                            }
                            else
                            {
                                dataGridView1.Rows.Add(reader.GetInt32(1), reader.GetString(2), reader.GetString(3), ActionImage.SetImage(ActionImage.iconSettingOff), reader.GetString(5));
                            }
                        }
                    }
                }
                connection.Close();
            }

        }
        private void CheckDetailService(string serviceName)
        {
            if(ActionService.CheckServiceExists(serviceName)){
                using (SQLiteConnection connection = new SQLiteConnection(ActionService.connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Services SET install = @install, status = @Status, description = @Description WHERE Name = @Name";
                    using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@install", true);
                        updateCommand.Parameters.AddWithValue("@Status", ActionService.CheckDetailStatusService(serviceName));
                        updateCommand.Parameters.AddWithValue("@Description", ActionService.GetDescriptionService(serviceName));
                        updateCommand.Parameters.AddWithValue("@Name", serviceName);
                        updateCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection(ActionService.connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Services SET install = @install, status = @Status WHERE Name = @Name";
                    using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@install", false);
                        updateCommand.Parameters.AddWithValue("@Status", "");
                        updateCommand.Parameters.AddWithValue("@Name", serviceName);
                        updateCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
        }
        private void Openfile(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
                openFileDialog.Title = "Chọn tệp exe";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.Multiselect = false;
                openFileDialog.FileName = string.Empty;

                string name;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedExePath = openFileDialog.FileName;
                    string parentDirectory = Path.GetDirectoryName(selectedExePath);
                    string configFilePath = Path.Combine(parentDirectory, "config.txt");
                    if (File.Exists(configFilePath))
                    {
                        name = File.ReadAllText(configFilePath);
                    }
                    else
                    {
                        MessageBox.Show("The file was not found in the selected folder.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (ActionService.CheckServiceExistsInListManager(name))
                    {
                        MessageBox.Show("This service is already in the management list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    using (SQLiteConnection connection = new SQLiteConnection(ActionService.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string insertQuery = "INSERT INTO Services (install,name,description,path) VALUES (@Install,@Name,@Description,@Path)";
                            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Install", true);
                                insertCommand.Parameters.AddWithValue("@Name", name);
                                insertCommand.Parameters.AddWithValue("@Description", " ");
                                insertCommand.Parameters.AddWithValue("@Path", parentDirectory);
                                insertCommand.ExecuteNonQuery();
                            }
                            connection.Close();
                        }catch(SQLiteException ex)
                        {
                            MessageBox.Show(ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    ActionService.InstallerService(parentDirectory);
                    Thread.Sleep(1000);
                    listToGridView();
                    
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn tệp tin exe.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void dataGridView1_OnClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            bool install = Convert.ToBoolean(row.Cells[0].Value);
            string path = row.Cells[4].Value.ToString();
            string name = row.Cells[1].Value.ToString();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    //install
                    if (install)
                    {
                        ActionService.UninstallerService(path);
                        MessageBox.Show($"Bạn đã go cai dat {name}");
                        if (detailService != null)
                        {
                            detailService.Close();
                            detailService = null;
                            panel1.Controls.Clear();
                        }
                        listToGridView();
                    }
                    else
                    {
                        ActionService.InstallerService(path);
                        MessageBox.Show($"Bạn đã cai dat {name}");
                        listToGridView();
                    }
                }else if (e.ColumnIndex == 3)
                {
                    // action start or stop
                    if (install)
                    {
                        if (ActionService.CheckStatusService(name))
                        {
                            ActionService.StopService(name);
                        }
                        else
                        {
                            ActionService.StartService(name);
                        }
                        listToGridView();
                        OpenDetailService(name);
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show($"Service {name} is not installed, do you want to install it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            ActionService.InstallerService(path);
                            MessageBox.Show($"Bạn đã cai dat {name}");
                            listToGridView();
                        }
                    }
                }
                else
                {
                    // show detail
                    if (install)
                    {
                        OpenDetailService(name);
                    }
                    else
                    {
                        MessageBox.Show("Service not installed!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                
            }
        }
        private void dataGridView1_DeleteService(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                DialogResult result = MessageBox.Show($"Do you want to remove {row.Cells[1].Value.ToString()} from the management list?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(ActionService.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string deleteQuery = "DELETE FROM Services WHERE name = @Name";
                            using (SQLiteCommand insertCommand = new SQLiteCommand(deleteQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Name", row.Cells[1].Value.ToString());
                                insertCommand.ExecuteNonQuery();
                            }
                            connection.Close();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    listToGridView();

                }
            }
        }
        private void OpenDetailService(string nameService)
        {
            if(detailService != null)
            {
                detailService.Close();
                detailService = null;
                panel1.Controls.Clear();
            }
            detailService = new DetailService(this,nameService);
            detailService.TopLevel = false;
            detailService.FormBorderStyle = FormBorderStyle.None;
            detailService.Dock = DockStyle.Fill;
            panel1.Controls.Add(detailService);
            detailService.Show();
        }
        private void RefreshData(object sender, EventArgs e)
        {
            listToGridView();
        }
    }
}
