using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Xml;
using System.Management;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        bool alive = true;
        object responseLock = new object();
        object requestLock = new object();

        public Service1()
        {
            InitializeComponent();
        }

        #region task1
        private static void WriteLog(string z)
        {
            using (StreamWriter F = new StreamWriter("C:\\LABA4\\Lab4.log", true))
            {
                F.WriteLine(DateTime.Now + z);
            }
        }
        static void StartLog(object source, EventArrivedEventArgs e)
        {
            WriteLog(" files created");
        }
        static void StopLog(object source, EventArrivedEventArgs e)
        {
            WriteLog(" files created");
        }

        private void Task1()
        {
            ManagementScope S = new ManagementScope(@"\\.\ROOT\cimv2");
            // SELECT * FROM __InstanceCreationEvent WITHIN 5 WHERE TargetInstance ISA 'CIM_DirectoryContainsFile' AND TargetInstance.GroupComponent = Win32_Directory.Name='C:\\\\LABA4\\\\TEST' // subscribtion for event creation in table win32_IP4PersistedRouteTable
            //WqlEventQuery Q1 = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 5 WHERE TargetInstance ISA 'CIM_DirectoryContainsFile' AND TargetInstance.GroupComponent=" @"'Win32_Directory.Name=""C:\\\\LABA4\\\\TEST""'"); // WITHIN 1
            WqlEventQuery Q1 = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 5 WHERE TargetInstance ISA 'CIM_DirectoryContainsFile' AND TargetInstance.GroupComponent=" +
                @"'Win32_Directory.Name=""C:\\\\LABA4\\\\TEST""'"); // WITHIN 1
            ManagementEventWatcher W1 = new ManagementEventWatcher(Q1);
            W1.EventArrived += new EventArrivedEventHandler(StartLog);
            W1.Start();


            WqlEventQuery Q2 = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 5 WHERE TargetInstance ISA 'CIM_DirectoryContainsFile' AND TargetInstance.GroupComponent=" +
                @"'Win32_Directory.Name=""C:\\\\LABA4\\\\TEST""'");
            ManagementEventWatcher W2 = new ManagementEventWatcher(Q2);
            W2.EventArrived += new EventArrivedEventHandler(StopLog);
            W2.Start();
        }
        #endregion

        protected override void OnStart(string[] args)
        {
            Task1();



            alive = true;
            Thread thread = new Thread(Server);
            thread.Start();
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Users\troll\Desktop\lab4\Request");
            if (!directoryInfo.Exists) directoryInfo.Create();
            directoryInfo = new DirectoryInfo(@"C:\Users\troll\Desktop\lab4\Response");
            if (!directoryInfo.Exists) directoryInfo.Create();
        }

        protected override void OnStop()
        {
            alive = false;
        }

        #region ClearDirectory
        void ClearAllDirectory()
        {
            string parth = @"D:\3year\1sem\Sysprog\lab4\Server XML\";
            ClearDirectory(parth + "Request");
            ClearDirectory(parth + "Response");
        }
        void ClearDirectory(string parth)
        {
            if (Directory.Exists(parth))
            {
                foreach (string fileName in Directory.GetFiles(parth, "*.xml"))
                {
                    File.Delete(fileName);
                }
            }
        }
        #endregion

        void Server()
        {
            while (alive)
            {
                IPAddress ip = IPAddress.Parse("192.168.49.149");
                IPEndPoint thisIpEnd = new IPEndPoint(ip, 51000);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                try
                {
                    socket.Bind(thisIpEnd);
                    while (alive)
                    {
                        EndPoint endPoint = new IPEndPoint(ip, 0);
                        byte[] buffer = new byte[512];
                        int length = socket.ReceiveFrom(buffer, ref endPoint);
                        if (length > buffer.Length)
                        {
                            length = buffer.Length;
                        }
                        string res = Encoding.Unicode.GetString(buffer, 0, length);
                        res = WorckWithData(res);
                        buffer = Encoding.Unicode.GetBytes(res);
                        socket.SendTo(buffer, endPoint);
                    }
                }
                catch (Exception e)
                {
                    using (StreamWriter logWrite = new StreamWriter(@"C:\Users\troll\Desktop\lab4\UDP.log", true))
                    {
                        logWrite.WriteLine(DateTime.Now + "  " + e.Message);
                    }
                }
            }
        }

        string WorckWithData(string data)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(WriteDataInRequest(data));
                string request = document.InnerText;
                ManagementObjectSearcher[] searcher = null;
                if (request == "Kyiv_Time")
                {
                    searcher = new ManagementObjectSearcher[] { new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LocalTime") };
                }
                else
                {
                    if (request == "London_Time")
                    {
                        searcher = new ManagementObjectSearcher[] { new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UTCTime") };
                    }
                    else
                    {
                        if (request == "KyivAndLondon")
                        {
                            searcher = new ManagementObjectSearcher[] {new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LocalTime"),
                                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UTCTime") };
                        }
                    }
                }
                string parth = WriteDataInResponse();
                using (XmlTextWriter respouse = new XmlTextWriter(parth, null))
                {
                    respouse.WriteStartDocument();
                    respouse.WriteStartElement("Response");
                    if (searcher != null)
                    {
                        if (searcher.Length == 1)
                        {
                            respouse.WriteStartElement("City");
                            respouse.WriteString(request.Replace("_Time", ""));
                            respouse.WriteEndElement();
                            Writer(respouse, searcher[0]);
                        }
                        else
                        {
                            respouse.WriteStartElement("Kyiv");
                            Writer(respouse, searcher[0]);
                            respouse.WriteEndElement();
                            respouse.WriteStartElement("London");
                            Writer(respouse, searcher[1]);
                            respouse.WriteEndElement();
                        }
                    }
                    else
                    {
                        respouse.WriteString("Incorect");
                    }
                    respouse.WriteEndElement();
                    respouse.WriteEndDocument();
                }
                return File.ReadAllText(parth);
            }
            catch (Exception e)
            {
                using (StreamWriter logWrite = new StreamWriter(@"C:\Users\troll\Desktop\lab4\UDP.log", true))
                {
                    logWrite.WriteLine(DateTime.Now + "  " + e.Message);
                }
            }
            return null;
        }

        void Writer(XmlTextWriter respouse, ManagementObjectSearcher searcher)
        {
            foreach (ManagementObject queryObj in searcher.Get())
            {
                WriteSoloElem(respouse, queryObj, "Day", 1);
                WriteSoloElem(respouse, queryObj, "DayOfWeek", 1);
                WriteSoloElem(respouse, queryObj, "Hour");
                WriteSoloElem(respouse, queryObj, "Milliseconds");
                WriteSoloElem(respouse, queryObj, "Minute");
                WriteSoloElem(respouse, queryObj, "Month", 1);
                WriteSoloElem(respouse, queryObj, "Second");
                WriteSoloElem(respouse, queryObj, "Year", 2000);
            }
        }

        void WriteSoloElem(XmlTextWriter respouse, ManagementObject queryObj, string key, int def = 0)
        {
            respouse.WriteStartElement(key);
            respouse.WriteString(queryObj[key]?.ToString() ?? def.ToString());
            respouse.WriteEndElement();
        }

        string WriteDataInResponse()
        {
            string parth = @"C:\Users\troll\Desktop\lab4\Response\";
            lock (responseLock)
            {
                Regex regex = new Regex(@"C:\\Users\\troll\\Desktop\\lab4\\Response\\Responce-(?<num>\d+)\.xml");
                int max = 0;
                foreach (string fileName in Directory.GetFiles(parth, "*.xml"))
                {
                    foreach (Match match in regex.Matches(fileName))
                    {
                        int temp = int.Parse(match.Groups["num"].Value);
                        if (temp > max) max = temp;
                    }
                }
                parth = parth + $"Responce-{max + 1}.xml";
                File.Create(parth).Close();
            }
            return parth;
        }

        string WriteDataInRequest(string data)
        {
            string parth = @"C:\Users\troll\Desktop\lab4\Request\";
            lock (requestLock)
            {
                Regex regex = new Regex(@"C:\\Users\\troll\\Desktop\\lab4\\Request\\Request-(?<num>\d+)\.xml");
                int max = 0;
                foreach (string fileName in Directory.GetFiles(parth, "*.xml"))
                {
                    foreach (Match match in regex.Matches(fileName))
                    {
                        int temp = int.Parse(match.Groups["num"].Value);
                        if (temp > max) max = temp;
                    }
                }
                parth = parth + $"Request-{max + 1}.xml";
                FileStream file = File.Create(parth);
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(data);
                }
                file.Close();
            }
            return parth;
        }
    }
}
