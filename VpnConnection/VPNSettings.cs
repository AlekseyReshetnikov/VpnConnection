using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ARvpn
{
    public class VpnItem
    {
        public string ipMask = "192.168.248.";
        public string vpnIP;
        public string vpnName;
        public string vpnUser;
        public string Routes = "192.168.0.0";
        public void SetVpnPwd(string p) { vpnPwd = p; }
        internal string vpnPwd;
    }

    public class VPNSettings
    {
        public string SettingsPath;
        public VPNConnect Connect;
        private VpnItem CurrentVpn;

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
            /*XmlNodeList Ns = D.DocumentElement.ChildNodes;
            XmlNode N;
            if (Ns.Count == 0)
            {
                N = D.CreateElement("S");
                D.DocumentElement.AppendChild(N);
            }
            else { N = Ns[0]; }
            SaveParam(D, N);
            */
            XmlNode vpnNode = D.DocumentElement.SelectSingleNode("vpnConnect");
            if (vpnNode == null)
            {
                vpnNode = D.CreateElement("vpnConnect");
                D.DocumentElement.AppendChild(vpnNode);
            }
            else
            {
                vpnNode.RemoveAll();
            }
            foreach (VpnItem item in Connect.VpnDict.Values)
            {
                CurrentVpn = item;
                XmlNode N = D.CreateElement("item");
                SaveParam(D, N);
                vpnNode.AppendChild(N);
            }
            var A = D.CreateAttribute("Current");
            A.Value = Connect.CurrentVpn.vpnName;
            vpnNode.Attributes.SetNamedItem(A);
            D.Save(SettingsPath);
        }

        public bool SettingsChanged = false;
        public bool LoadSettings(string s1 = "")
        { // Чтение настроечных параметров
            SettingsChanged = false;
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
                        XmlNode vpnNode = D.DocumentElement.SelectSingleNode("vpnConnect");
                        if (vpnNode != null)
                        {
                            XmlNodeList items = vpnNode.SelectNodes("item");
                            if (items.Count == 0) { vpnNode = null; }
                        }
                        if (vpnNode == null)
                        {
                            LoadParam(new AXMLNode(Ns[0]));
                            SettingsChanged = true;
                            Connect.VpnDict.Clear();
                            Connect.VpnDict.Add(CurrentVpn.vpnName, CurrentVpn);
                        }
                        else
                        {
                            XmlNodeList items = vpnNode.SelectNodes("item");
                            Connect.VpnDict.Clear();
                            VpnItem first = null;
                            foreach (XmlNode item in items)
                            {
                                var aitem = new AXMLNode(item);
                                LoadParam(aitem);
                                if (first == null) { first = CurrentVpn; }
                                try
                                {
                                    Connect.VpnDict.Add(CurrentVpn.vpnName, CurrentVpn);
                                }
                                catch { }
                            }
                            if (first != null)
                            {
                                CurrentVpn = first;
                            }
                            try
                            {

                                string current = (string)vpnNode.Attributes["Current"].Value;
                                if (Connect.VpnDict.TryGetValue(current, out first))
                                {
                                    CurrentVpn = first;
                                }
                                Connect.CurrentVpn = CurrentVpn;

                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            D = null;
            if (SettingsChanged)
            {
                SaveSettings();
            }
            return true;
        }

        const string sProtected = "Protected";

        void LoadParam(AXMLNode N)
        {
            bool Protected = N[sProtected] == "1";
            Func<string, string> F;
            if (Protected)
            {
                F = (x) => { string s = N[x]; if (s.StartsWith(sProtected + "!")) { s = Unprotect(s.Substring(sProtected.Length + 1)); }; return s; };
            }
            else
            {
                F = (x) => N[x];
            }
            CurrentVpn = new VpnItem();
            CurrentVpn.vpnName = N["vpnName"];
            CurrentVpn.vpnUser = N["vpnUser"];
            CurrentVpn.vpnPwd = F("vpnPwd");
            CurrentVpn.ipMask = N["ipMask"];
            CurrentVpn.Routes = N["Routes"];

            if (!Protected)
            {
                SettingsChanged = true;
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
            P = (n, v) => { string s = v; if (!string.IsNullOrEmpty(s)) { s = sProtected + "!" + Protect(s); } SetItem(n, s); };
            SetItem("Protected", "1");
            SetItem("vpnName", CurrentVpn.vpnName);
            SetItem("vpnUser", CurrentVpn.vpnUser);
            P("vpnPwd", CurrentVpn.vpnPwd);
            SetItem("ipMask", CurrentVpn.ipMask);
            SetItem("Routes", CurrentVpn.Routes);
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
            try
            {
                byte[] ClearBytes = ProtectedData.Unprotect(ProtData, null, DataProtectionScope.LocalMachine);
                string s = Encoding.UTF8.GetString(ClearBytes);
                return s;
            }
            catch
            {
                return "";
            }
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
