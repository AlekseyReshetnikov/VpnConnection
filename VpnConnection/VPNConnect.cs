using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ARvpn
{
    class VPNConnect
    {
        public string ipMask= "192.168.248.";
        public string vpnIP;
        public string vpnName;
        public string vpnUser;
        public void SetVpnPwd(string p) { vpnPwd = p; }
        private string vpnPwd;
        public string SettingsPath;
        public string getVpnIP()
        {
            string s = Dns.GetHostName();
            string svn = "";
            IPHostEntry ipE = Dns.GetHostEntry(s);
            IPAddress[] al = ipE.AddressList;
            foreach (IPAddress a in al)
            {
                string si = a.ToString();
                if (si.StartsWith(ipMask)) { svn = si; }
            }
            vpnIP = svn;
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

        public void routeAdd()
        {
            string svn = getVpnIP();
            if (!string.IsNullOrEmpty(svn))
            {
                string cmd =
    @"route ADD 192.168.0.0 MASK 255.255.0.0 " + svn + @"
pause";
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
            ipMask = "192.168.248.";
            vpnName = "VPN-подключение";
            vpnUser = "aresh@norma.com";
        }

        internal void ShowParam()
        {
            FormParam F = new ARvpn.FormParam();
            F.txtVPNName.Text = vpnName;
            F.txtVpnUser.Text = vpnUser;
            F.txtPwd.Text = vpnPwd;
            F.txtVpnMask.Text = ipMask;
            //F.Show();
            bool focused = F.txtVPNName.Focus();
            DialogResult r = F.ShowDialog();
            if (r == DialogResult.OK) { SaveSettings(); }
        }

        public void rasdial()
        {
            string s = string.Format("{0} {1} {2}", vpnName, vpnUser, vpnPwd);
            Process p = System.Diagnostics.Process.Start("rasdial.exe",  s); //VPNConnectionName VPNUsername VPNPassword
            p.WaitForExit();
        }

        public void SaveSettings()
        {
            //s = Application.StartupPath; s += @"\Settings.xml";
            XmlDocument D = new XmlDocument();
            Boolean F = File.Exists(SettingsPath);
            if (F)
            {
                try
                {
                    D.Load(SettingsPath);
                }
                catch { F = false; }
            }
            if (!F) { D.LoadXml(@"<?xml version=""1.0"" encoding=""windows-1251""?><Option/>"); }
            XmlNodeList Ns = D.DocumentElement.ChildNodes;
            XmlNode N;
            if (Ns.Count == 0)
            {
                N = D.CreateElement("S");
                D.DocumentElement.AppendChild(N);
            }
            else { N = Ns[0]; }
            SaveParam(D, N);
            D.Save(SettingsPath);
        }

        public bool LoadSettings(string s1 = "")
        { // Чтение настроечных параметров
            string s, s2;
            if (s1 == "")
            { s = Application.StartupPath; s += @"\Settings.xml"; }
            else { s = s1; }
            SettingsPath = s;
            if (!File.Exists(s)) return false;
            XmlDocument D = new XmlDocument();
            try
            {
                D.Load(s);
                XmlNodeList Ns = D.DocumentElement.ChildNodes;
                if (Ns.Count > 0)
                {
                    var AC = Ns[0].Attributes;
                    var A = AC[@"Path"];
                    if (A != null)
                    {
                        s2 = A.Value;
                    }
                    else { s2 = ""; }
                    if ((s1 == "") && (s2 != "")) { LoadSettings(s2); }
                    else
                    {
                        LoadParam(new AXMLNode(Ns[0]));
                    }
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            D = null;
            return true;
        }

        const string sProtected = "Protected";

        void LoadParam(AXMLNode N)
        {
            bool Protected = N[sProtected]=="1";
            Func<string, string> F;
            if(Protected){
                F = (x) => { string s = N[x]; if (s.StartsWith(sProtected+"!")) { s = Unprotect(s.Substring(sProtected.Length + 1)); }; return s; };
            } else
            {
                F = (x) => N[x];
            }
            vpnName = N["vpnName"];
            vpnUser = N["vpnUser"];
            vpnPwd = F("vpnPwd");
            ipMask = N["ipMask"];
            if (!Protected)
            {
                SaveSettings();
            }
        }

        void SaveParam(XmlDocument D, XmlNode N)
        {
            Action<string, string> SetItem = (p, ConName) =>
            {
                 var A = D.CreateAttribute(p);
                 A.Value = ConName;
                 N.Attributes.SetNamedItem(A);
             };
            Action<string, string> P;
            P = (n, v) => { string s = v; if (!string.IsNullOrEmpty(s)) { s = sProtected + "!" + Protect(s); } SetItem(n, s);};
            SetItem("Protected","1");
            SetItem("vpnName",vpnName);
            SetItem("vpnUser", vpnUser);
            P("vpnPwd", vpnPwd);
            SetItem("ipMask", ipMask);
        }

        string Protect(string data)
        {
            byte[] ClearBytes = Encoding.UTF8.GetBytes(data);
            byte[] ProtData =
   ProtectedData.Protect(ClearBytes, null, DataProtectionScope.LocalMachine);
            string s = Convert.ToBase64String(ProtData);
            return s;
        }

        string Unprotect(string data)
        {
            byte[] ProtData = Convert.FromBase64String(data);
            byte[] ClearBytes = ProtectedData.Unprotect(ProtData, null, DataProtectionScope.LocalMachine);
            string s =Encoding.UTF8.GetString(ClearBytes);
            return s;
        }

        public class AXMLNode
        {
            public XmlNode N;
            public AXMLNode(XmlNode AN)
            {
                N = AN;
            }
            public string this[string s, string d = ""] {
                get {
                    XmlAttribute A1 = N.Attributes[s];
                    string r;
                    if (A1 != null) { r = A1.Value; } else r = d;
                    return r;
                }
            }
        }
    }
}
