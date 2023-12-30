using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Remote_Desktop
{
    public class XMLHandler
    {
        XmlDocument doc = new XmlDocument();
        private static XMLHandler localXmlHandler;
        private static XMLHandler remoteXmlHandler;
        HostChooseForm hostChooseForm = new HostChooseForm();
        private XMLHandler(string configFileName)
        {
            doc.Load(configFileName);
        }

        public static XMLHandler SwitchXMLHandler(bool localTrueRemoteFalse, string configFileName)
        {
            if (localTrueRemoteFalse)
            {
                if (localXmlHandler == null)
                {
                    localXmlHandler = new XMLHandler(configFileName);
                }
                return localXmlHandler;
            }
            else
            {
                if (remoteXmlHandler == null)
                {
                    remoteXmlHandler = new XMLHandler(configFileName);
                }
                return remoteXmlHandler;
            }
        }
        public List<string> GetNodeAttributeValueList(XmlNode xmlNode, string attribute)
        {
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;
            List<string> itemList = new List<string>();
            try
            {
                foreach (XmlNode xn in xmlNodeList)
                {
                    itemList.Add(xn.Attributes[attribute].Value);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return itemList;
        }
        public XmlNode FindXmlNodeByHostName(string pcName, bool isConnectButton)
        {
            XmlNode xmlNode = null;
            if (string.IsNullOrWhiteSpace(pcName))
            {
                MessageBox.Show("Please enter the computer name firstly.");
                return null;
            }
            else if (pcName.Contains("."))
            {
                if (!Regex.IsMatch(pcName,
                    @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                {
                    MessageBox.Show("The Computer Name entered is incorrect, please confirm.");
                    return null;
                }
                else
                {
                    xmlNode = doc.DocumentElement.SelectSingleNode(string.Format("/configuration/factory/equipmenttype/ipconfig[@ipaddress='{0}']", pcName));
                    if (xmlNode == null)
                    {
                        if (isConnectButton)
                        {
                            if (MessageBox.Show("The Computer Name is not registered yet,please register first.\nElse you can Remote Desktop by input Host Info.", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                ProcessHandler.StartProcesses(@"C:\WINDOWS\system32\mstsc.exe");
                            }
                        }
                        else
                        {
                            MessageBox.Show("The Computer Name is not registered yet,please register first.");
                        }
                    }
                    return xmlNode;
                }
            }
            else
            {
                List<string> selectPcNameList = new List<string>();
                List<string> factoryList = GetNodeAttributeValueList(FindXmlNodeByXPath(string.Format("/configuration")), "name");
                foreach (string factory in factoryList)
                {
                    List<string> equipmentList = GetNodeAttributeValueList(FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']", factory)), "name");
                    foreach (string equipment in equipmentList)
                    {
                        List<string> pcNameList = GetNodeAttributeValueList(FindXmlNodeByXPath(string.Format("/configuration/factory[@name='{0}']/equipmenttype[@name='{1}']", factory, equipment)), "pcname");
                        foreach (string item in pcNameList)
                        {
                            if (item.IndexOf(pcName, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            {
                                selectPcNameList.Add(item);
                            }

                        }
                    }
                }

                if (selectPcNameList.Count == 0)
                {
                    if (isConnectButton)
                    {
                        if (MessageBox.Show("The Computer Name is not registered yet, please register first.\nElse you can Remote Desktop by input Host Info.", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            ProcessHandler.StartProcesses(@"C:\WINDOWS\system32\mstsc.exe");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Computer Name is not registered yet, please register first.");
                    }
                    return null;
                }
                else if (selectPcNameList.Count == 1)
                {
                    xmlNode = doc.DocumentElement.SelectSingleNode(string.Format("/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']", selectPcNameList[0]));
                    return xmlNode;
                }
                else if (selectPcNameList.Count > 10)
                {
                    MessageBox.Show("There are too many Host Info to match the requirement, please input in details.");
                    return null;
                }
                else
                {
                    Point point = new Point(25, 40);
                    hostChooseForm.Width = 0;
                    for (int i = 0; i < selectPcNameList.Count; i++)
                    {
                        hostChooseForm.Width += 125;
                        Button button = new Button();
                        button.Height = 40;
                        button.Width = 100;
                        button.BackColor = Color.LightBlue;
                        button.Text = selectPcNameList[i];
                        button.Location = point;
                        button.TabIndex = i;
                        point.X += 125;
                        hostChooseForm.Controls.Add(button);
                        button.Click += new EventHandler(hostChooseForm.btn_click);
                    }
                    hostChooseForm.Width -= 95;
                    hostChooseForm.StartPosition = FormStartPosition.CenterScreen;
                    hostChooseForm._pcName = String.Empty;
                    hostChooseForm.ShowDialog();
                    xmlNode = doc.DocumentElement.SelectSingleNode(string.Format("/configuration/factory/equipmenttype/ipconfig[@pcname='{0}']", hostChooseForm._pcName));
                    hostChooseForm.Controls.Clear();
                    return xmlNode;
                }
            }
        }

        public XmlNode FindXmlNodeByXPath(string xPath)
        {
            XmlElement configuration = doc.DocumentElement;
            if (xPath == "")
            {
                return configuration;
            }
            XmlNode xmlNode = configuration.SelectSingleNode(xPath);
            return xmlNode;
        }

        public void SaveXMLFile(string fileName)
        {
            doc.Save(fileName);
        }

        public string GetXMLAttributesValue(XmlNode xmlNode, string attributeName)
        {
            try
            {
                return xmlNode.Attributes[attributeName].Value;
            }
            catch (Exception)
            {
                return string.Empty;
                throw;
            }


        }

        public void SetXMLAttributesValue(XmlNode xmlNode, string attributeName, string attributeValue)
        {
            try
            {
                xmlNode.Attributes[attributeName].Value = attributeValue;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public XmlNode CreateXMLElement(XmlNode parentNode, string elementName, List<string> attributeNames, List<string> attributeValues)
        {
            XmlElement xmlElement = doc.CreateElement(elementName);
            for (int i = 0; i < attributeNames.Count; i++)
            {
                xmlElement.SetAttribute(attributeNames[i], attributeValues[i]);
            }

            parentNode.AppendChild(xmlElement);
            return xmlElement;
        }

        public void DeleteXMLElement(XmlNode xmlNode)
        {
            XmlNode parentNode = xmlNode.ParentNode;
            parentNode.RemoveChild(xmlNode);
        }

        public XmlDocument GetXmlDocument()
        {
            return doc;
        }
    }
}
