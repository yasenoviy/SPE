using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Timer timeTimer;
        public Form1()
        {
            InitializeComponent();
        }

        bool IfIpValide()
        {
            string regStr = @"^(((25[0-5]|2[0-4][0-9]|[0-1]([0-9]{2})|[0-9]{1,2})\.){3}(25[0-5]|2[0-4][0-9]|[0-1]([0-9]{2})|[0-9]{1,2}))$";
            Regex regex = new Regex(regStr);
            if (regex.IsMatch(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "");
                return true;
            }
            else
            {
                errorProvider1.SetError(textBox1, "Incorect IP");
                return false;
            }
        }

        #region ClearDirectory
        void ClearAllDirectory()
        {
            string parth = @"Client/";
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


        string WriteDataInRequest()
        {
            string parth = @"Client\Request\";
            Regex regex = new Regex(@"Client\\Request\\Request-(?<num>\d+)\.xml");
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
            File.Create(parth).Close();
            return parth;
        }

        string WriteDataInResponse(string data)
        {
            string parth = @"Client\Response\";
            Regex regex = new Regex(@"Client\\Response\\Response-(?<num>\d+)\.xml");
            int max = 0;
            foreach (string fileName in Directory.GetFiles(parth, "*.xml"))
            {
                foreach (Match match in regex.Matches(fileName))
                {
                    int temp = int.Parse(match.Groups["num"].Value);
                    if (temp > max) max = temp;
                }
            }
            parth = parth + $"Response-{max + 1}.xml";
            FileStream file = File.Create(parth);
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine(data);
            }
            file.Close();
            return parth;
        }

        string SendMessegeAndGetResponce(string message)
        {
            byte[] messege = Encoding.Unicode.GetBytes(message);
            byte[] buffer = new byte[2048];
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(textBox1.Text), 51000);
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                try
                {
                    socket.Connect(server);
                    socket.Send(messege);
                    int length = socket.Receive(buffer);
                    return Encoding.Unicode.GetString(buffer, 0, length);
                }
                catch (Exception exp)
                {
                    label4.Text += exp.Message;
                }
            }
            return null;
        }
        
        DataTable GetData(XmlDocument document)
        {
            DataTable res = new DataTable();
            res.Columns.Add("Місто", typeof(string));
            res.Columns.Add("Дата", typeof(string));
            res.Columns.Add("Час", typeof(string));
            DateTime timeKiev = ParseTime(document, "Response/Kyiv").GetDate();
            DateTime timeLondon = ParseTime(document, "Response/London").GetDate();
            res.Rows.Add("Київ", timeKiev.ToString("d"), timeKiev.ToString("t"));
            res.Rows.Add("Лондон", timeLondon.ToString("d"), timeLondon.ToString("t"));
            return res;
        }

        #region Parse
        SYSTEMTIME ParseTime(XmlDocument document, string prefix)
        {
            XPathNavigator navigator = document.CreateNavigator();
            SYSTEMTIME res = new SYSTEMTIME();
            res.wYear = ParseUint(navigator, prefix + "/Year", 2000);
            res.wDay = ParseUint(navigator, prefix + "/Day", 1);
            res.wDayOfWeek = ParseUint(navigator, prefix + "/DayOfWeek", 1);
            res.wHour = ParseUint(navigator, prefix + "/Hour");
            res.wMilliseconds = ParseUint(navigator, prefix + "/Milliseconds");
            res.wMinute = ParseUint(navigator, prefix + "/Minute");
            res.wMonth = ParseUint(navigator, prefix + "/Month");
            res.wSecond = ParseUint(navigator, prefix + "/Second");
            return res;
        }

        ushort ParseUint(XPathNavigator navigator,string xparth,ushort def = 0)
        {
            ushort res;
            string value = null;
            foreach (var i in navigator.Select(xparth)) value = i.ToString();
            return ushort.TryParse(value, out res) ? res : def;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("F");
            timeTimer = new Timer { Interval = 1000 };
            timeTimer.Tick += (object obj, EventArgs exp) => label2.Text = DateTime.Now.ToString("F");
            timeTimer.Start();
            DirectoryInfo directory = new DirectoryInfo(@"Client\Request");
            if (!directory.Exists)
            {
                directory.Create();
            }
            directory = new DirectoryInfo(@"Client\Response");
            if (!directory.Exists)
            {
                directory.Create();
            }
            ClearAllDirectory();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timeTimer.Stop();
            timeTimer.Dispose();
        }

        #region Button

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            dataGridView1.DataSource = null;
            if (IfIpValide())
            {
                string parth = WriteDataInRequest();
                using (XmlTextWriter writer = new XmlTextWriter(parth, null))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Request");
                    writer.WriteString(checkBox1.Checked? "London_Time": "Kyiv_Time");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                string responce = SendMessegeAndGetResponce(File.ReadAllText(parth));
                if (responce != null)
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(WriteDataInResponse(responce));
                    SYSTEMTIME syst = ParseTime(document, "Response");
                    string city = null;
                    foreach (var i in document.CreateNavigator().Select("Response/City")) city = i.ToString();
                    label4.Text = $"{city} time {syst.GetDate().ToString("F")} try to set! ";
                    try
                    {
                        SetLocalTime(ref syst);
                    }catch(Exception ex)
                    {
                        label4.Text += "\n"+ex.Message;
                    }
                }
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            dataGridView1.DataSource = null;
            if (IfIpValide())
            {
                string parth = WriteDataInRequest();
                using(XmlTextWriter writer = new XmlTextWriter(parth, null))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Request");
                    writer.WriteString("KyivAndLondon");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                string responce = SendMessegeAndGetResponce(File.ReadAllText(parth));
                if (responce != null)
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(WriteDataInResponse(responce));
                    dataGridView1.DataSource = GetData(document);
                    for(int i=0; i < dataGridView1.Columns.Count;i++)
                    {
                        dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                }
            }
        }

        #endregion
        #region SYSTIME

        [DllImport("kernel32.dll")]
        private extern static uint SetLocalTime(ref SYSTEMTIME lpSystemTime);


        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
            public DateTime GetDate()
            {
                return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
            }
        }
        #endregion
    }
}
