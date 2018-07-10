using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARvpn
{
    public partial class MainForm : Form
    {
        VPNConnect VpnConnect;
        public MainForm()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (!SelectItem())
            {
                string k = cmbItems.Text;
                VpnConnect.CurrentVpn = new VPNConnect.VpnItem() { vpnName = k };
            }
            if (VpnConnect.ShowParam()) {
                LoadVpns();
            }
        }

        private void btnVpnConnect_Click(object sender, EventArgs e)
        {
            if (SelectItem())
            {
                VpnConnectAsync();
            }
        }
        async void VpnConnectAsync()
        {
            await Task.Run(() =>
            {
                VpnConnect.rasdial();
                VpnConnect.routeAdd();
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            VpnConnect = new VPNConnect();
            if (!VpnConnect.LoadSettings())
            {
                VpnConnect.init();
                VpnConnect.SaveSettings();
            }
            LoadVpns();
        }
        private void LoadVpns()
        { 
            foreach(var k in VpnConnect.VpnDict.Keys)
            {
                cmbItems.Items.Add(k);
            }
            cmbItems.SelectedItem = VpnConnect.CurrentVpn.vpnName;
        }

        private bool SelectItem()
        {
            string k = (string)cmbItems.SelectedItem;
            if (k == null) { k = cmbItems.Text; }
            VPNConnect.VpnItem item=null;
            var res = VpnConnect.VpnDict.TryGetValue(k, out item );
            if (res)
            {
                VpnConnect.CurrentVpn = item;
            }
            return res;
        }

    }
}
