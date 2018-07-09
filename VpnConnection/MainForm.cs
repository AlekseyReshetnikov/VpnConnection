using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
            VpnConnect = new VPNConnect();
            if (!VpnConnect.LoadSettings()) {
                VpnConnect.init();
                VpnConnect.SaveSettings();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            VpnConnect.ShowParam();
        }

        private void btnVpnConnect_Click(object sender, EventArgs e)
        {
            VpnConnectAsync();
        }
        async void VpnConnectAsync()
        {
            await Task.Run(() =>
            {
                VpnConnect.rasdial();
                VpnConnect.routeAdd();
            });
        }

    }
}
