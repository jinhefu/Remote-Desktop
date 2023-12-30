using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remote_Desktop
{
    class FTPHandler
    {
        string userName = null;
        string password = null;
        string ftpTargetDir = null;
        public FTPHandler(string userName, string password, string ftpTargetDir)
        {
            this.userName = userName;
            this.password = password;
            this.ftpTargetDir = ftpTargetDir;
        }

        public string getFTPFileVersion(string ftpFileName)
        {
            WebResponse webresp = null;
            StreamReader ftpFileListReader = null;
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(this.ftpTargetDir + ftpFileName));
                ftpRequest.UseBinary = true;
                ftpRequest.KeepAlive = false;
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                webresp = ftpRequest.GetResponse();
                ftpFileListReader = new StreamReader(webresp.GetResponseStream(), Encoding.UTF8);
                StringBuilder str = new StringBuilder();
                string line = ftpFileListReader.ReadLine();
                while (line != null)
                {
                    str.Append(line).Append("/");
                    line = ftpFileListReader.ReadLine();
                }
                string[] fullname = str.ToString().Split('/');
                foreach (string name in fullname)
                {
                    if (name.Contains(ftpFileName))
                    {
                        //02-14-19  01:10PM                61440 IP_Config.xml
                        return name.Substring(0, 17);
                    }
                    else
                    {
                        continue;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Connect FTP Failed, please check the internet." + e.Message);
                return null;
            }
            finally
            {
                if (ftpFileListReader != null)
                {
                    ftpFileListReader.Close();
                }
                if (webresp != null)
                {
                    webresp.Close();
                }
            }

        }
        public void downloadFTPFile(string localFileName, string ftpFileName)
        {
            FileStream outputStream = null;
            FtpWebResponse response = null;
            Stream ftpStream = null;
            try
            {
                FtpWebRequest downRequest = (FtpWebRequest)WebRequest.Create(new Uri(this.ftpTargetDir + ftpFileName));
                downRequest.UseBinary = true;
                downRequest.KeepAlive = false;
                downRequest.Credentials = new NetworkCredential(userName, password);
                downRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                response = (FtpWebResponse)downRequest.GetResponse();
                ftpStream = response.GetResponseStream();
                outputStream = new FileStream(localFileName, FileMode.Create);
                int bufferSize = 1024 * 1024;
                byte[] buffer = new byte[bufferSize];
                int readLen = ftpStream.Read(buffer, 0, bufferSize);
                while (readLen > 0)
                {
                    outputStream.Write(buffer, 0, readLen);
                    readLen = ftpStream.Read(buffer, 0, bufferSize);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Connect FTP Failed, please check the internet." + e.Message);
            }
            finally
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }

                if (response != null)
                {
                    response.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
            }
        }

        public bool uploadFTPFile(string localFileName, string ftpFileName)
        {

            FileInfo localFile = new FileInfo(localFileName);
            FileStream localFileStream = null;
            Stream requestStream = null;
            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(this.ftpTargetDir + ftpFileName));
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.ContentLength = localFile.Length;
                requestStream = ftpWebRequest.GetRequestStream();
                int bufferLength = (int)localFile.Length;
                byte[] buffer = new byte[bufferLength];
                localFileStream = localFile.OpenRead();
                int readLen = localFileStream.Read(buffer, 0, bufferLength);
                while (readLen != 0)
                {
                    requestStream.Write(buffer, 0, bufferLength);
                    requestStream.Flush();
                    readLen = localFileStream.Read(buffer, 0, bufferLength);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Connect FTP Failed, please check the internet." + e.Message);
                return false;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
                if (localFileStream != null)
                {
                    localFileStream.Close();
                }
            }
        }
    }
}
