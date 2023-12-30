using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Remote_Desktop
{
    public partial class MainWindow : Form
    {
        private List<string> resolutionList = new List<string>() { "全屏", "1920 x 1080", "1680 x 1050", "1400 x 1050", "1440 x 900", "1366 x 768", "1280 x 1024", "1280 x 800", "1280 x 768", "1280 x 720", "1024 x 768", "800 x 600", "640 x 480" };
        private XMLHandler xmlHandler;
        private FTPHandler ftpHandler;
        private IniHandler iniHandler;
        private string remoteFileName = "Remote_Config.xml";
        private string localFileName = "Personal_Config.xml";
        private bool inputTrueGroupFalse = true;
        private bool localTrueRemoteFalse = true;
        private bool ftpEnable = false;
        public MainWindow()
        {
            initialConfig();
            InitializeComponent();
            initialControlsShow();
            localTrueRemoteFalse = true;
            xmlHandler = XMLHandler.SwitchXMLHandler(localTrueRemoteFalse, localFileName);
        }


        private void initialConfig()
        {
            try
            {
                iniHandler = new IniHandler(System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + "custom_config.ini");
                ftpEnable = bool.Parse(iniHandler.GetValue("ftp.enable", "ftp"));
                if (ftpEnable)
                {
                    ftpHandler = new FTPHandler(iniHandler.GetValue("ftp.username", "ftp"), iniHandler.GetValue("ftp.password", "ftp"), iniHandler.GetValue("ftp.targetDir", "ftp"));
                    ftpHandler.downloadFTPFile(System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + remoteFileName, remoteFileName);
                    if (!File.Exists(remoteFileName))
                    {
                        MessageBox.Show("The remote configuration file is not found, please confirm.");
                        throw new Exception();
                    }
                    xmlHandler = XMLHandler.SwitchXMLHandler(false, remoteFileName);
                }
                if (!File.Exists(localFileName))
                {
                    MessageBox.Show("The local configuration file is not found, please confirm.");
                }
                xmlHandler = XMLHandler.SwitchXMLHandler(true, localFileName);
            }
            catch (Exception)
            {
                MessageBox.Show("The associated configuration file is abnormal, please confirm.");
            }
        }


        /**
         * inital controls status
         */
        private void initialControlsShow()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Remote Desktop";
            this.MaximizeBox = false;
            // default height
            this.Height = 320;
            // default display out of height 
            this.gbx_ByGroup.Location = new Point(8, 600);
            this.gbx_HostInfo.Location = new Point(8, 600);
            this.rad_ByInput.Hide();
            this.rad_ByGroup.Hide();
            // default use input
            this.rad_ByInput.Checked = true;
            this.toolTip1.SetToolTip(btn_ShowGroup, "Group Info Connect");
            this.txt_PcName.Enabled = false;
            this.txt_IPAddress.Enabled = false;
            this.txt_Password.Enabled = false;
            this.txt_UserName.Enabled = false;
            this.txt_Port.Enabled = false;

            cbo_Resolution.DataSource = resolutionList;
            this.txt_RemoteComputer.Focus();
        }
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            inputTrueGroupFalse = rad_ByInput.Checked ? true : false;
            XmlNode xmlNode = null;
            if (inputTrueGroupFalse)
            {
                xmlNode = xmlHandler.FindXmlNodeByHostName(txt_RemoteComputer.Text.Trim(), true);
            }
            else
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']",
                    cbo_PcName.SelectedItem));
            }
            if (xmlNode == null)
            {
                return;
            }

            uint screenMode = 2;
            string resolution = "0,3,0,0,1920,1040";
            if (cbo_Resolution.SelectedItem.ToString().Contains("全屏"))
            {
                screenMode = 2;
            }
            else
            {
                screenMode = 1;
                string[] resolutions = cbo_Resolution.SelectedItem.ToString().Split(new char[] { 'x' }, 2);
                resolution = string.Format("0,0,0,0,{0},{1}", resolutions[0].Trim(), resolutions[1].Trim());
            }
            string pcName = String.Empty;
            string ipAddress = String.Empty;
            string userName = String.Empty;
            string password = String.Empty;
            string port = String.Empty;
            try
            {
                pcName = xmlHandler.GetXMLAttributesValue(xmlNode, "pcname");
                ipAddress = xmlHandler.GetXMLAttributesValue(xmlNode, "ipaddress");
                userName = xmlHandler.GetXMLAttributesValue(xmlNode, "username");
                password = xmlHandler.GetXMLAttributesValue(xmlNode, "password");
                port = xmlHandler.GetXMLAttributesValue(xmlNode, "port");
            }
            catch (Exception)
            {
                MessageBox.Show("The Host Info in XML file is abnormal, please confirm.");
            }

            if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(port))
            {
                MessageBox.Show("The Host regeister Info is incorrect, please confirm.");
                return;
            }
            string[] widthAndHeight = resolution.Split(new char[] { ',' }, 6);

            #region WriteRDP
            string connectName = (String.Empty.Equals(pcName) ? "Temp" : pcName) + ".rdp";
            using (FileStream fileStream = new FileStream(connectName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine("smart sizing:i:1");
                    streamWriter.WriteLine("use multimon:i:0");
                    streamWriter.WriteLine("session bpp:i:32");
                    streamWriter.WriteLine("compression:i:1");
                    streamWriter.WriteLine("keyboardhook:i:2");
                    streamWriter.WriteLine("audiocapturemode:i:0");
                    streamWriter.WriteLine("videoplaybackmode:i:1");
                    streamWriter.WriteLine("connection type:i:7");
                    streamWriter.WriteLine("networkautodetect:i:1");
                    streamWriter.WriteLine("bandwidthautodetect:i:1");
                    streamWriter.WriteLine("displayconnectionbar:i:1");
                    streamWriter.WriteLine("enableworkspacereconnect:i:0");
                    streamWriter.WriteLine("disable wallpaper:i:0");
                    streamWriter.WriteLine("allow font smoothing:i:0");
                    streamWriter.WriteLine("allow desktop composition:i:0");
                    streamWriter.WriteLine("disable full window drag:i:1");
                    streamWriter.WriteLine("disable menu anims:i:1");
                    streamWriter.WriteLine("disable themes:i:0");
                    streamWriter.WriteLine("disable cursor setting:i:0");
                    streamWriter.WriteLine("bitmapcachepersistenable:i:1");
                    streamWriter.WriteLine("audiomode:i:0");
                    streamWriter.WriteLine("redirectprinters:i:1");
                    streamWriter.WriteLine("redirectcomports:i:0");
                    streamWriter.WriteLine("redirectsmartcards:i:1");
                    streamWriter.WriteLine("redirectclipboard:i:1");
                    streamWriter.WriteLine("redirectposdevices:i:0");
                    streamWriter.WriteLine("autoreconnection enabled:i:1");
                    streamWriter.WriteLine("authentication level:i:0");
                    streamWriter.WriteLine("prompt for credentials:i:0");
                    streamWriter.WriteLine("negotiate security layer:i:1");
                    streamWriter.WriteLine("remoteapplicationmode:i:0");
                    streamWriter.WriteLine("alternate shell:s:");
                    streamWriter.WriteLine("shell working directory:s:");
                    streamWriter.WriteLine("gatewayhostname:s:");
                    streamWriter.WriteLine("gatewayusagemethod:i:4");
                    streamWriter.WriteLine("gatewaycredentialssource:i:4");
                    streamWriter.WriteLine("gatewayprofileusagemethod:i:0");
                    streamWriter.WriteLine("promptcredentialonce:i:0");
                    streamWriter.WriteLine("gatewaybrokeringtype:i:0");
                    streamWriter.WriteLine("use redirection server name:i:0");
                    streamWriter.WriteLine("rdgiskdcproxy:i:0");
                    streamWriter.WriteLine("kdcproxyname:s:");
                    streamWriter.WriteLine("drivestoredirect:s:");
                    streamWriter.WriteLine("domain:s:");
                    streamWriter.WriteLine("screen mode id:i:" + screenMode);
                    streamWriter.WriteLine("desktopwidth:i:" + widthAndHeight[4]);
                    streamWriter.WriteLine("desktopheight:i:" + widthAndHeight[5]);
                    streamWriter.WriteLine("winposstr:s:" + resolution);
                    streamWriter.WriteLine("full address:s:" + ipAddress);
                    streamWriter.WriteLine("username:s:" + userName);
                    streamWriter.WriteLine("Server Port:i:" + port);
                    streamWriter.WriteLine("password 51:b:" + Encryption.Encrypt(password).Replace("-", ""));
                    streamWriter.Flush();
                    fileStream.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
            #endregion

            FileInfo fileInfo = new FileInfo(connectName);
            ProcessHandler.StartProcesses(connectName);
            Thread.Sleep(5000);
            fileInfo.Delete();
        }

        private void btn_ShowGroup_Click(object sender, EventArgs e)
        {
            if (btn_ShowGroup.Name == "btn_ShowGroup" && this.btn_HostInfo.Name == "btn_HostInfo")
            {
                this.btn_ShowGroup.Name = "btn_ShowGroup_Packup";
                this.btn_ShowGroup.BackgroundImage = Remote_Desktop.Properties.Resources.up;
                this.Height += 150;
                rad_ByInput.Show();
                rad_ByGroup.Show();
                gbx_ByGroup.Location = new Point(8, 225);
                pan_ButtomButton.Location = new Point(8, 355);

            }
            else if (btn_ShowGroup.Name == "btn_ShowGroup" && this.btn_HostInfo.Name == "btn_HostInfo_Packup")
            {
                this.btn_ShowGroup.Name = "btn_ShowGroup_Packup";
                this.btn_ShowGroup.BackgroundImage = Remote_Desktop.Properties.Resources.up;
                this.Height += 150;
                rad_ByInput.Show();
                rad_ByGroup.Show();
                gbx_ByGroup.Location = new Point(8, 225);
                gbx_HostInfo.Location = new Point(8, 360);
                pan_ButtomButton.Location = new Point(8, 540);
            }
            else if (btn_ShowGroup.Name == "btn_ShowGroup_Packup" && this.btn_HostInfo.Name == "btn_HostInfo_Packup")
            {
                rad_ByInput.Checked = true;
                this.btn_ShowGroup.Name = "btn_ShowGroup";
                this.btn_ShowGroup.BackgroundImage = Remote_Desktop.Properties.Resources.right;
                this.Height -= 150;
                rad_ByInput.Hide();
                rad_ByGroup.Hide();
                gbx_HostInfo.Location = new Point(8, 225);
                pan_ButtomButton.Location = new Point(8, 395);
                gbx_ByGroup.Location = new Point(8, 600);

                txt_PcName.Text = string.Empty;
                txt_IPAddress.Text = string.Empty;
                txt_UserName.Text = string.Empty;
                txt_Password.Text = string.Empty;
                txt_Port.Text = string.Empty;
                return;
            }
            else
            {
                rad_ByInput.Checked = true;
                this.btn_ShowGroup.Name = "btn_ShowGroup";
                this.btn_ShowGroup.BackgroundImage = Remote_Desktop.Properties.Resources.right;
                this.Height -= 150;
                rad_ByInput.Hide();
                rad_ByGroup.Hide();
                gbx_HostInfo.Location = new Point(8, 600);
                pan_ButtomButton.Location = new Point(8, 208);
                gbx_ByGroup.Location = new Point(8, 600);
                return;
            }

            try
            {
                List<string> factoryList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration")), "name");
                factoryList.Sort();
                cbo_Factory.DataSource = factoryList;
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
        }

        #region ComboBoxInitialize

        private void cbo_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> equipmentList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']", cbo_Factory.SelectedItem.ToString())), "name");
                equipmentList.Sort();
                cbo_EquipmentType.DataSource = equipmentList;
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
        }

        private void cbo_EquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbo_EquipmentType.SelectedItem == null)
            {
                cbo_PcName.DataSource = null;
                return;
            }
            try
            {
                List<string> pcNameList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']/equipmenttype[@name='{1}']", cbo_Factory.SelectedItem.ToString(), cbo_EquipmentType.SelectedItem.ToString())), "pcname");
                pcNameList.Sort();
                cbo_PcName.DataSource = pcNameList;
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
        }

        #endregion

        private void btn_HostInfo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_RemoteComputer.Text) && rad_ByInput.Checked == true && btn_HostInfo.Name == "btn_HostInfo")
            {
                MessageBox.Show("Please enter the computer name firstly.");
                return;
            }
            if (this.btn_HostInfo.Name == "btn_HostInfo" && this.btn_ShowGroup.Name == "btn_ShowGroup")
            {
                XmlNode xmlNode = initialTxt_HostInfoUseInputOrGroup(false);
                if (xmlNode == null)
                {
                    return;
                }
                this.Height += 200;
                gbx_HostInfo.Location = new Point(8, 225);
                pan_ButtomButton.Location = new Point(8, 395);
                this.gbx_ByGroup.Location = new Point(8, 600);
                this.btn_HostInfo.Name = "btn_HostInfo_Packup";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.collapse;

            }
            else if (this.btn_HostInfo.Name == "btn_HostInfo" && this.btn_ShowGroup.Name == "btn_ShowGroup_Packup")
            {
                XmlNode xmlNode = initialTxt_HostInfoUseInputOrGroup(false);
                if (xmlNode == null)
                {
                    return;
                }
                //this.Height += 150;
                this.Height += 200;
                gbx_HostInfo.Location = new Point(8, 360);
                pan_ButtomButton.Location = new Point(8, 540);
                this.btn_HostInfo.Name = "btn_HostInfo_Packup";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.collapse;
            }
            else if (this.btn_HostInfo.Name == "btn_HostInfo_Packup" && this.btn_ShowGroup.Name == "btn_ShowGroup_Packup")
            {
                this.Height -= 200;
                //gbx_HostInfo.Location = new Point(8, 441);
                //pan_ButtomButton.Location = new Point(8, 208);
                gbx_HostInfo.Location = new Point(8, 600);

                pan_ButtomButton.Location = new Point(8, 355);

                this.btn_HostInfo.Name = "btn_HostInfo";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.expand;

                txt_PcName.Text = string.Empty;
                txt_IPAddress.Text = string.Empty;
                txt_UserName.Text = string.Empty;
                txt_Password.Text = string.Empty;
                txt_Port.Text = string.Empty;
            }
            else
            {
                this.Height -= 200;
                gbx_HostInfo.Location = new Point(8, 441);
                pan_ButtomButton.Location = new Point(8, 208);

                this.btn_HostInfo.Name = "btn_HostInfo";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.expand;

                txt_PcName.Text = string.Empty;
                txt_IPAddress.Text = string.Empty;
                txt_UserName.Text = string.Empty;
                txt_Password.Text = string.Empty;
                txt_Port.Text = string.Empty;
            }
        }


        #region ToolStripMenuEvent
        private void tsm_DetailInfo_Click(object sender, EventArgs e)
        {
            XmlNode xmlNode = getXmlNodeByTSMSource(sender);
            if (xmlNode == null)
            {
                return;
            }
            try
            {
                txt_PcName.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "pcname");
                txt_IPAddress.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "ipaddress");
                txt_UserName.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "username");
                txt_Password.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "password");
                txt_Port.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "port");
            }
            catch (Exception)
            {
                MessageBox.Show("The Host Info in XML file is abnormal, please confirm.");
            }

            if (this.btn_HostInfo.Name == "btn_HostInfo" && this.btn_ShowGroup.Name == "btn_ShowGroup")
            {
                this.Height += 200;
                gbx_HostInfo.Location = new Point(8, 225);
                pan_ButtomButton.Location = new Point(8, 395);
                this.gbx_ByGroup.Location = new Point(8, 600);
                this.btn_HostInfo.Name = "btn_HostInfo_Packup";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.collapse;
            }
            else if (this.btn_HostInfo.Name == "btn_HostInfo" && this.btn_ShowGroup.Name == "btn_ShowGroup_Packup")
            {
                //this.Height += 150;
                this.Height += 200;
                gbx_HostInfo.Location = new Point(8, 360);
                pan_ButtomButton.Location = new Point(8, 540);
                this.btn_HostInfo.Name = "btn_HostInfo_Packup";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.collapse;
            }
        }
        private void tsm_Modify_Click(object sender, EventArgs e)
        {
            XmlNode xmlNode = getXmlNodeByTSMSource(sender);
            HostInfoForm hostInfoForm = new HostInfoForm(xmlHandler, localTrueRemoteFalse ? localFileName : remoteFileName);
            if (xmlNode == null)
            {
                return;
            }
            hostInfoForm.ShowHostInfo(xmlNode);
            if (hostInfoForm.getWhetherModify())
            {
                uploadFTPFile();
            }
        }
        private void tsm_Register_Click(object sender, EventArgs e)
        {
            HostInfoForm hostInfoForm = new HostInfoForm(xmlHandler, localTrueRemoteFalse ? localFileName : remoteFileName);
            hostInfoForm.RegisterNewHost();
            hostInfoForm.ShowDialog();
            if (hostInfoForm.getWhetherModify() == false)
            {
                return;
            }
            try
            {

                List<string> factoryList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration")), "name");
                factoryList.Sort();
                cbo_Factory.DataSource = factoryList;
                uploadFTPFile();
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
        }
        private void tsm_DeleteHost_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you really sure you want to delete it,rather than modify it.", "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            XmlNode xmlNode = getXmlNodeByTSMSource(sender);
            if (xmlNode == null)
            {
                return;
            }
            xmlHandler.DeleteXMLElement(xmlNode);
            xmlHandler.SaveXMLFile(localTrueRemoteFalse ? localFileName : remoteFileName);
            try
            {
                List<string> pcNameList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']/equipmenttype[@name='{1}']", cbo_Factory.SelectedItem.ToString(), cbo_EquipmentType.SelectedItem.ToString())), "pcname");
                pcNameList.Sort();
                cbo_PcName.DataSource = pcNameList;
                uploadFTPFile();
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
        }
        private void tsm_DeleteType_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comfirm to delete it, rather than modify it.", "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
            XmlNode xmlNode = null;
            if (tsm.Name.Contains("_Input"))
            {
                xmlNode = xmlHandler.FindXmlNodeByHostName(txt_RemoteComputer.Text.Trim(), false);
                if (xmlNode == null)
                {
                    return;
                }
                XmlNode parentNode = xmlNode.ParentNode;
                xmlHandler.DeleteXMLElement(parentNode);
            }
            else if (tsm.Name.Contains("_Group"))
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory/equipmenttype[@name='{0}']",
                    cbo_EquipmentType.SelectedItem));
                if (xmlNode == null)
                {
                    return;
                }
                xmlHandler.DeleteXMLElement(xmlNode);
            }
            else
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']",
                    txt_PcName.Text));
                if (xmlNode == null)
                {
                    return;
                }
                XmlNode parentNode = xmlNode.ParentNode;
                xmlHandler.DeleteXMLElement(parentNode);
            }

            xmlHandler.SaveXMLFile(localTrueRemoteFalse ? localFileName : remoteFileName);

            try
            {
                List<string> equipmentList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']", cbo_Factory.SelectedItem.ToString())), "name");
                equipmentList.Sort();
                cbo_EquipmentType.DataSource = equipmentList;
                uploadFTPFile();
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
        }
        private void tsm_DeleteGroup_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you really sure you want to delete it, rather than modify it.", "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
            XmlNode xmlNode = null;
            if (tsm.Name.Contains("_Input"))
            {
                xmlNode = xmlHandler.FindXmlNodeByHostName(txt_RemoteComputer.Text.Trim(), false);
                if (xmlNode == null)
                {
                    return;
                }
                XmlNode parentNode = xmlNode.ParentNode;
                XmlNode grandParentNode = parentNode.ParentNode;
                xmlHandler.DeleteXMLElement(grandParentNode);
            }
            else if (tsm.Name.Contains("_Group"))
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory[@name='{0}']",
                    cbo_Factory.SelectedItem));
                if (xmlNode == null)
                {
                    return;
                }
                xmlHandler.DeleteXMLElement(xmlNode);
            }
            else
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']",
                    txt_PcName.Text));
                if (xmlNode == null)
                {
                    return;
                }
                XmlNode parentNode = xmlNode.ParentNode;
                XmlNode grandParentNode = parentNode.ParentNode;
                xmlHandler.DeleteXMLElement(grandParentNode);
            }

            xmlHandler.SaveXMLFile(localTrueRemoteFalse ? localFileName : remoteFileName);
            try
            {
                List<string> factoryList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration")), "name");
                factoryList.Sort();
                cbo_Factory.DataSource = factoryList;
                uploadFTPFile();
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
        }

        #endregion

        private void linklbl_Edit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.txt_IPAddress.Enabled = true;
            this.txt_Password.Enabled = true;
            this.txt_UserName.Enabled = true;
            this.txt_Port.Enabled = true;
        }

        private void linklbl_Save_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_PcName.Text))
            {
                MessageBox.Show("Please make sure PcName textbox isn't empty.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_UserName.Text) || string.IsNullOrWhiteSpace(txt_Password.Text))
            {
                MessageBox.Show("UserName or Password hasn't been entered yet.");
                return;
            }
            if (!Regex.IsMatch(txt_IPAddress.Text,
                @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
            {
                MessageBox.Show("The IP address is not valid, please confirm.");
                return;
            }
            if (!Regex.IsMatch(txt_Port.Text,
                @"^([1-6]?[1-9]{0,3}[0-9])$"))
            {
                MessageBox.Show("The Port number is not valid, please confirm.");
                return;
            }
            XmlNode xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']", txt_PcName.Text));
            if (xmlNode == null)
            {
                return;
            }
            try
            {
                xmlHandler.SetXMLAttributesValue(xmlNode, "ipaddress", txt_IPAddress.Text.Trim());
                xmlHandler.SetXMLAttributesValue(xmlNode, "username", txt_UserName.Text.Trim());
                xmlHandler.SetXMLAttributesValue(xmlNode, "password", txt_Password.Text.Trim());
                xmlHandler.SetXMLAttributesValue(xmlNode, "port", txt_Port.Text.Trim());
            }
            catch (Exception)
            {
                MessageBox.Show("The Host Info in XML file is abnormal, please confirm.");
            }
            xmlHandler.SaveXMLFile(localTrueRemoteFalse ? localFileName : remoteFileName);

            this.txt_IPAddress.Enabled = false;
            this.txt_Password.Enabled = false;
            this.txt_UserName.Enabled = false;
            this.txt_Port.Enabled = false;
            uploadFTPFile();
        }

        private void linklbl_HostInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txt_PcName.Text = string.Empty;
            txt_IPAddress.Text = string.Empty;
            txt_UserName.Text = string.Empty;
            txt_Password.Text = string.Empty;
            txt_Port.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(txt_RemoteComputer.Text) && rad_ByInput.Checked == true)
            {
                MessageBox.Show("Please enter the computer name firstly.");
                return;
            }

            XmlNode xmlNode = initialTxt_HostInfoUseInputOrGroup(false);
            if (xmlNode == null)
            {
                return;
            }

            if (this.btn_HostInfo.Name == "btn_HostInfo" && this.btn_ShowGroup.Name == "btn_ShowGroup")
            {
                this.Height += 200;
                gbx_HostInfo.Location = new Point(8, 225);
                pan_ButtomButton.Location = new Point(8, 395);
                this.gbx_ByGroup.Location = new Point(8, 600);
                this.btn_HostInfo.Name = "btn_HostInfo_Packup";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.collapse;
            }
            else if (this.btn_HostInfo.Name == "btn_HostInfo" && this.btn_ShowGroup.Name == "btn_ShowGroup_Packup")
            {
                this.Height += 200;
                gbx_HostInfo.Location = new Point(8, 360);
                pan_ButtomButton.Location = new Point(8, 540);
                this.btn_HostInfo.Name = "btn_HostInfo_Packup";
                this.btn_HostInfo.BackgroundImage = Remote_Desktop.Properties.Resources.collapse;
            }

        }

        private XmlNode initialTxt_HostInfoUseInputOrGroup(bool whetherSkipInput)
        {
            inputTrueGroupFalse = rad_ByInput.Checked ? true : false;
            XmlNode xmlNode = null;
            if (inputTrueGroupFalse)
            {
                if (whetherSkipInput)
                {
                    return null;
                }
                xmlNode = xmlHandler.FindXmlNodeByHostName(txt_RemoteComputer.Text.Trim(), false);
            }
            else
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']",
                    cbo_PcName.SelectedItem));
            }
            if (xmlNode == null)
            {
                return null;
            }
            try
            {
                txt_PcName.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "pcname");
                txt_IPAddress.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "ipaddress");
                txt_UserName.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "username");
                txt_Password.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "password");
                txt_Port.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "port");
            }
            catch (Exception)
            {
                MessageBox.Show("The Host Info in XML file is abnormal, please confirm.");
            }
            return xmlNode;
        }

        private XmlNode getXmlNodeByTSMSource(object sender)
        {
            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
            XmlNode xmlNode = null;
            if (tsm.Name.Contains("_Input"))
            {
                xmlNode = xmlHandler.FindXmlNodeByHostName(txt_RemoteComputer.Text.Trim(), false);
            }
            else if (tsm.Name.Contains("_Group"))
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']",
                    cbo_PcName.SelectedItem));
            }
            else
            {
                xmlNode = xmlHandler.FindXmlNodeByXPath(string.Format(
                    "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']",
                    txt_PcName.Text));
            }
            return xmlNode;
        }


        private void MainWindow_Activated(object sender, EventArgs e)
        {
            txt_RemoteComputer.Focus();
        }

        private void cbo_PcName_SelectedIndexChanged(object sender, EventArgs e)
        {
            initialTxt_HostInfoUseInputOrGroup(true);
        }


        private void txt_RemoteComputer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.btn_Connect_Click(sender, e);
            }
        }
        private void linklbl_ChangeConfig_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if ("Remote_Config".Equals(linklbl_ChangeConfig.Text))
            {
                localTrueRemoteFalse = true;
                linklbl_ChangeConfig.Text = "Personal_Config";
                xmlHandler = XMLHandler.SwitchXMLHandler(localTrueRemoteFalse, localFileName);                
            }
            else if ("Personal_Config".Equals(linklbl_ChangeConfig.Text))
            {
                localTrueRemoteFalse = false;
                linklbl_ChangeConfig.Text = "Remote_Config";
                xmlHandler = XMLHandler.SwitchXMLHandler(localTrueRemoteFalse, remoteFileName);                
            }
            List<string> factoryList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration")), "name");
            factoryList.Sort();
            cbo_Factory.DataSource = factoryList;
        }

        private void uploadFTPFile()
        {
            if (ftpEnable)
            {
                ftpHandler.uploadFTPFile(System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + remoteFileName, remoteFileName);
            }
        }
    }
}
