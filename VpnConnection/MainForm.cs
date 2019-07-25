using System;
using System.Drawing;
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
                VpnConnect.CurrentVpn = new VpnItem() { vpnName = k };
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
        bool VpnConnectTaskExecuting = false;
        async void VpnConnectAsync()
        {
            if (VpnConnectTaskExecuting)
            {
                ApText0("Уже запущено подключение. Подождите", Color.Red);Apln();
                return;
            }
            prBar.Visible = true;
            Progress(0, 2);
            VpnConnectTaskExecuting = true;
            ApText0( DateTime.Now.ToString()+" ", Color.Black);
            ApText0("Запускаю подключение...", Color.DarkBlue); Apln();
            Action<string> mes = (x) => { ApText(x);};
            Task VpnConnectTask = Task.Run(() =>
            {
                try
                {
                    try
                    {
                        VpnConnect.rasdial();
                        Progress(1);
                        this.Invoke(mes,"routeAdd");
                        VpnConnect.routeAdd();
                        Progress(2);
                        this.Invoke(mes, "Подключились");
                    }
                    finally
                    {
                        VpnConnectTaskExecuting = false;
                        this.Invoke(new Action(() => { prBar.Visible = false; }));

                    }
                }
                catch(Exception E) { this.Invoke(new Action(() => { ApText0(E.Message, Color.Red); })); }
            });
            await VpnConnectTask;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            VpnConnect = new VPNConnect();
            if (!VpnConnect.Settings.LoadSettings())
            {
                VpnConnect.init();
                VpnConnect.Settings.SaveSettings();
            }
            LoadVpns();
            ApText0(DateTime.Now.ToString()+"  ",Color.DarkTurquoise);
            ApText("Start");
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
            VpnItem item=null;
            var res = VpnConnect.VpnDict.TryGetValue(k, out item );
            if (res)
            {
                VpnConnect.CurrentVpn = item;
            }
            return res;
        }

        public void Progress(int pos, int max = 0)
        {
            Action a = () =>
            {
                if (pos > prBar.Maximum) pos = prBar.Maximum;
                prBar.Value = pos;
                if (max > 0) prBar.Maximum = max;
            };
            if (this.InvokeRequired)
            {
                this.Invoke(a);
            }
            else
            {
                a();
            }

        }

        private void ApText(string txt)
        {
            rtxtLog.AppendText(txt);
            Apln();
        }
        private void ApText0(string txt, Color font_color)
        {
            var richTextBox1 = rtxtLog;
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = font_color;
            richTextBox1.AppendText(txt);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }
        private void Apln()
        {
            rtxtLog.AppendText("\n");
            rtxtLog.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SelectItem())
            {
                ApText( VpnConnect.getVpnIP());
            }
        }

        private void btnRouteAdd_Click(object sender, EventArgs e)
        {
            if (SelectItem())
            {
                ApText(VpnConnect.routeAddCmd());
            }
        }

        private void btnRC_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"mstsc.exe");
        }
    }
}
