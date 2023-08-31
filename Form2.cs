using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace WindownformInstallerService
{
    public partial class DetailService : Form
    {
        private string  _nameService;
        private Form1 _parentForm;
        public DetailService(Form1 parentForm,string nameService)
        {
            InitializeComponent();
            this._nameService = nameService;
            this._parentForm = parentForm;
        }
        private void DetailService_Load(object sender, EventArgs e)
        {
            InitState(sender, e);
            NameService.Text = _nameService;
        }
        private void InitState(object sender, EventArgs e)
        {
            try
            {
                using (ServiceController serviceController = new ServiceController(_nameService))
                {
                    _nameService = serviceController.ServiceName;
                    descriptionValue.Text = serviceController.DisplayName;
                    if (serviceController.Status == ServiceControllerStatus.Running ||
                            serviceController.Status == ServiceControllerStatus.StartPending)
                    {
                        // ddang chay
                        start.Text = "Stop";
                        restart.Visible = true;
                    }
                    else
                    {
                        start.Text = "Start";
                        restart.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Start(object sender, EventArgs e)
        {
            if (ActionService.CheckStatusService(_nameService))
            {
                ActionService.StopService(_nameService);
            }
            else
            {
                ActionService.StartService(_nameService);
            }
            InitState(sender, e);
            _parentForm.listToGridView();
        }
        private void Restart(object sender, EventArgs e)
        {
            ActionService.RestartService(_nameService);
            InitState(sender, e);
            _parentForm.listToGridView();
        }
    }
}
