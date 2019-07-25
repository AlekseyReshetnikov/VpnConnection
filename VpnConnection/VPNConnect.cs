using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ARvpn
{
    public class VPNConnect
    {
        public VPNSettings Settings;
        public VpnItem CurrentVpn = null;

        public Dictionary<string, VpnItem> VpnDict;
        public VPNConnect()
        {
            VpnDict = new Dictionary<string, VpnItem>();
            CurrentVpn = new VpnItem();
            Settings = new VPNSettings { Connect = this };
        }

        public string getVpnIP()
        {
            string s = Dns.GetHostName();
            string svn = "";
            IPHostEntry ipE = Dns.GetHostEntry(s);
            IPAddress[] al = ipE.AddressList;
            var masks = CurrentVpn.ipMask.Split(';');
            foreach (IPAddress a in al)
            {
                string si = a.ToString();
                bool f = false;
                foreach (var mask in masks)
                {
                    if (si.StartsWith(mask)) { svn = si; f = true; break; }
                }
                if (f) break;
            }
            CurrentVpn.vpnIP = svn;
            return svn;
        }

        public List<string> getAddressList()
        {
            List<string> r = new List<string>();
            string s = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostEntry(s);
            IPAddress[] al = ipE.AddressList;
            foreach (IPAddress a in al)
            {
                r.Add(a.ToString());
            }
            return r;
        }

        public string routeAddCmd()
        {
            string svn = getVpnIP();
            StringBuilder cmd = new StringBuilder();
            if (!string.IsNullOrEmpty(svn))
            {
                if (string.IsNullOrEmpty(CurrentVpn.Routes))
                {
                    cmd.AppendLine(@"route ADD 192.168.0.0 MASK 255.255.0.0 " + svn);
                } else foreach(var r in CurrentVpn.Routes.Split(';'))
                    cmd.AppendLine($"route ADD {r} MASK 255.255.0.0 {svn}");
                cmd.AppendLine(@"pause");
            }
            return cmd.ToString();
        }

        public void routeAdd()
        {
            string cmd = routeAddCmd();
            if (!string.IsNullOrEmpty(cmd))
            {
                StreamWriter sw = new StreamWriter("routeadd.cmd");
                sw.Write(cmd);
                sw.Flush();
                sw.Close();
                //System.Diagnostics.Process.Start("routeadd.cmd");
                Process proc = new Process();
                proc.StartInfo.FileName = "routeadd.cmd";
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
            }

        }

        public void init()
        {
            CurrentVpn.ipMask = "192.168.248.";
            CurrentVpn.vpnName = "VPN-подключение";
            CurrentVpn.vpnUser = "aresh@norma.com";
        }

        internal bool ShowParam()
        {
            bool res = false;
            FormParam F = new ARvpn.FormParam();
            F.txtVPNName.Text = CurrentVpn.vpnName;
            F.txtVpnUser.Text = CurrentVpn.vpnUser;
            F.txtPwd.Text = CurrentVpn.vpnPwd;
            F.txtVpnMask.Text = CurrentVpn.ipMask;
            F.txtRouteAdd.Text = CurrentVpn.Routes;
            //F.Show();
            bool focused = F.txtVPNName.Focus();
            DialogResult r = F.ShowDialog();
            if (r == DialogResult.OK)
            {
                bool newItem = !VpnDict.ContainsKey(CurrentVpn.vpnName);
                CurrentVpn.vpnName = F.txtVPNName.Text;
                CurrentVpn.vpnUser = F.txtVpnUser.Text;
                CurrentVpn.vpnPwd = F.txtPwd.Text;
                CurrentVpn.ipMask = F.txtVpnMask.Text;
                CurrentVpn.Routes = F.txtRouteAdd.Text;
                if (newItem)
                {
                    if (VpnDict.ContainsKey(CurrentVpn.vpnName))
                    {
                        VpnDict[CurrentVpn.vpnName] = CurrentVpn;
                    }
                    else
                    {
                        VpnDict.Add(CurrentVpn.vpnName, CurrentVpn);
                    }
                    res = true;
                }

                Settings.SaveSettings();
            }
            return res;
        }

        public void rasdial()
        {
            string s = string.Format("{0} {1} {2}", CurrentVpn.vpnName, CurrentVpn.vpnUser, CurrentVpn.vpnPwd);
            Process p = System.Diagnostics.Process.Start("rasdial.exe", s); //VPNConnectionName VPNUsername VPNPassword
            p.WaitForExit();
        }
    }
}
