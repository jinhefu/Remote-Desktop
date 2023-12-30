using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Remote_Desktop
{
    public partial class HostInfoForm : Form
    {
        private XMLHandler xmlHandler =null;
        private string xmlFileName = null;
        private bool whetherModify = false;
        public HostInfoForm(XMLHandler xmlHandler,string xmlFileName)
        {
            InitializeComponent();
            this.xmlHandler = xmlHandler;
            this.xmlFileName = xmlFileName;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.DialogResult = DialogResult.Cancel;
        }
        public void ShowHostInfo(XmlNode xmlNode)
        {
            txt_Factory.Hide();
            txt_EquipmentType.Hide();
            cbo_Factory.Hide();
            cbo_EquipmentType.Hide();
            cbo_PcName.Hide();
            lbl_Group.Hide();
            lbl_Type.Hide();
            lbl_Host.Hide();
            txt_PcName.Enabled = false;
            btn_Save.Text = "Update";
            try
            {
                txt_PcName.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "pcname");
                txt_IPAddress.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "ipaddress");
                txt_UserName.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "username");
                txt_Password.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "password");
                txt_Port.Text = xmlHandler.GetXMLAttributesValue(xmlNode, "port");
                this.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("The Host Info in XML file is abnormal, please confirm.");
            }
        }

        public bool getWhetherModify()
        {
            return whetherModify;
        }

        public void RegisterNewHost()
        {
            cbo_PcName.Hide();
            lbl_Host.Hide();
            txt_Factory.Hide();
            txt_EquipmentType.Hide();
            txt_PcName.Enabled = true;
            try
            {
                List<string> factoryList =
                    xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration")), "name");
                factoryList.Sort();
                factoryList.Add("-Create New-");
                cbo_Factory.DataSource = factoryList;
                List<string> equipmentTypeList =
                    xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']", cbo_Factory.SelectedItem.ToString())), "name");
                equipmentTypeList.Sort();
                equipmentTypeList.Add("-Create New-");
                cbo_EquipmentType.DataSource = equipmentTypeList;
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
            txt_Port.Text = "3389";
        }

        private void cbo_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_PcName.Text = string.Empty;
            txt_IPAddress.Text = string.Empty;
            txt_UserName.Text = string.Empty;
            txt_Password.Text = string.Empty;
            //txt_Port.Text = string.Empty;
            txt_Factory.Text = string.Empty;
            txt_EquipmentType.Text = string.Empty;
            txt_Factory.Hide();
            txt_EquipmentType.Hide();
            if (cbo_Factory.SelectedItem.ToString().Equals("-Create New-"))
            {
                cbo_EquipmentType.DataSource = new List<string>{("-Create New-")};
                txt_Factory.Show();
                txt_EquipmentType.Show();
                return;
            }

            List<string> equipmentTypeList = null;
            try
            {
                equipmentTypeList = xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']", cbo_Factory.SelectedItem.ToString())), "name");
                equipmentTypeList.Sort();
            }
            catch (Exception)
            {
                MessageBox.Show("The XML file content is abnormal, please confirm.");
            }
            equipmentTypeList.Add("-Create New-");
            cbo_EquipmentType.DataSource = equipmentTypeList;
        }

        private void cbo_EquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_PcName.Text = string.Empty;
            txt_IPAddress.Text = string.Empty;
            txt_UserName.Text = string.Empty;
            txt_Password.Text = string.Empty;
            //txt_Port.Text = string.Empty;
            txt_EquipmentType.Text = string.Empty;
            txt_EquipmentType.Hide();
            if (cbo_EquipmentType.SelectedItem.ToString().Equals("-Create New-"))
            {
                txt_EquipmentType.Show();
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
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

            if (this.btn_Save.Text == "Update")
            {
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
                    this.txt_IPAddress.Enabled = false;
                    this.txt_Password.Enabled = false;
                    this.txt_UserName.Enabled = false;
                    this.txt_Port.Enabled = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("The Host Info in XML file is abnormal, please confirm.");
                }
                
                xmlHandler.SaveXMLFile(xmlFileName);
                MessageBox.Show("Update success.");
                whetherModify = true;
            }

            if (this.btn_Save.Text == "Save")
            {
                XmlNode configuration = xmlHandler.FindXmlNodeByXPath("");
                XmlNode factoryElement = null;
                XmlNode equipmentTypElement = null;
                if (xmlHandler.FindXmlNodeByXPath(string.Format(
                        "/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']",
                        txt_PcName.Text.Trim())) != null)
                {
                    MessageBox.Show("The registered PcName already exists, please confirm.");
                    return;
                }
                if (cbo_Factory.SelectedItem.ToString() == "-Create New-")
                {
                    if (string.IsNullOrWhiteSpace(txt_Factory.Text) ||
                        string.IsNullOrWhiteSpace(txt_EquipmentType.Text))
                    {
                        MessageBox.Show("Please input Group or Type info for register.");
                        return;
                    }
                    if (xmlHandler.FindXmlNodeByXPath(string.Format(
                            "/configuration/factory[@name='{0}']",
                            txt_Factory.Text.Trim())) != null)
                    {
                        MessageBox.Show(xmlHandler.FindXmlNodeByXPath(string.Format(
                            "/configuration/factory[@name='{0}']",
                            txt_Factory.Text.Trim())).Attributes["name"].Value);
                        MessageBox.Show("The registered Group already exists, please confirm.");
                        return;
                    }
                    if (xmlHandler.FindXmlNodeByXPath(string.Format(
                            "/configuration/factory/equipmenttype[@name='{0}']",
                            txt_EquipmentType.Text.Trim())) != null)
                    {
                        MessageBox.Show("The registered Type already exists, please confirm.");
                        return;
                    }
                    factoryElement = xmlHandler.CreateXMLElement(configuration, "factory", new List<string>() { "name" }, new List<string>() { txt_Factory.Text.Trim()});
                    equipmentTypElement = xmlHandler.CreateXMLElement(factoryElement, "equipmenttype", new List<string>() { "name" }, new List<string>() { txt_EquipmentType.Text.Trim()});
                }
                else
                {
                    factoryElement = xmlHandler.FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']",
                        cbo_Factory.SelectedItem.ToString()));
                    if (cbo_EquipmentType.SelectedItem.ToString() == "-Create New-")
                    {
                        if (string.IsNullOrWhiteSpace(txt_EquipmentType.Text))
                        {
                            MessageBox.Show("Please input Type info for register.");
                            return;
                        }
                        if (xmlHandler.FindXmlNodeByXPath(string.Format(
                                "/configuration/factory/equipmenttype[@name='{0}']",
                                txt_EquipmentType.Text.Trim())) != null)
                        {
                            MessageBox.Show("The registered Type already exists, please confirm.");
                            return;
                        }
                        equipmentTypElement = xmlHandler.CreateXMLElement(factoryElement, "equipmenttype", new List<string>() { "name" }, new List<string>() { txt_EquipmentType.Text.Trim() });
                    }
                    else
                    {
                        equipmentTypElement = xmlHandler.FindXmlNodeByXPath(string.Format(
                            "/configuration/factory[@name='{0}']/equipmenttype[@name='{1}']",
                            cbo_Factory.SelectedItem.ToString(), cbo_EquipmentType.SelectedItem.ToString()));
                    }
                }
                XmlNode ipConfigElement = xmlHandler.CreateXMLElement(equipmentTypElement, "ipconfig",
                    new List<string>() {"pcname", "ipaddress", "username", "password", "port"},
                    new List<string>()
                    {
                        txt_PcName.Text.Trim(), txt_IPAddress.Text.Trim(),
                        txt_UserName.Text.Trim(), txt_Password.Text.Trim(),
                        txt_Port.Text.Trim()
                    });
                xmlHandler.SaveXMLFile(xmlFileName);

                MessageBox.Show("Save success.");
                whetherModify = true;
                List<string> factoryList = null;
                try
                {
                    factoryList =
                        xmlHandler.GetNodeAttributeValueList(xmlHandler.FindXmlNodeByXPath(string.Format("/configuration")), "name");
                    factoryList.Sort();
                    factoryList.Add("-Create New-");
                }
                catch (Exception)
                {
                    MessageBox.Show("The XML file content is abnormal, please confirm.");
                }
                cbo_Factory.DataSource = factoryList;
            }
        }
    }
}
