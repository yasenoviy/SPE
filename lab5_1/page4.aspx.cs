using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        string status;
        string ID;
        int Open = 0;
        bool fuck = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"];
            status = Request.QueryString["status"];
            L1.Text = "Результат перевірки TCP-служби з ID=" + ID;
            L1.Font.Size = 48;
            if (status == "1")
            {
                L2.Text = "ВЕРИФІКАЦІЮ EMAIL-АДРЕСИ УСПІШНО ЗАВЕРШЕНО";
                L2.ForeColor = System.Drawing.Color.Green;
                DateTime localDate = DateTime.Now;
                System.Threading.Thread.Sleep(3000);
                string Query = "select address, port from servise2 where ID = " + Request.QueryString["ID"];
                string DB = "Data Source = (LocalDB)\\MSSQLLocalDB; Initial Catalog = tcp; Integrated Security = True";
                string ipaddress = "";
                string temp = "";
                int port;
                string result = "";

                if (fuck)
                {
                    using (SqlConnection Conn = new SqlConnection(DB))
                    {
                        Conn.Open();
                        SqlCommand Comm = new SqlCommand(Query, Conn);
                        SqlDataReader R = Comm.ExecuteReader();
                        while (R.Read())
                        {
                            ipaddress = R[0].ToString();
                            temp = R[1].ToString();
                        }
                        port = Convert.ToInt32(temp);
                    }

                    IPAddress ipa = IPAddress.Parse(ipaddress);
                    try
                    {
                        Socket sock = new Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                        sock.Connect(ipa, port);
                        if (sock.Connected == true)
                        { // Port is in use and connection is successful
                            Open = 1; System.Threading.Thread.Sleep(1000);
                        }
                        sock.Close();
                        if (Open == 1)
                        {
                            L3.ForeColor = System.Drawing.Color.Green;
                            L3.Text = "Результат: TCP-служба працює";
                            result = "Work";
                        }
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        L3.ForeColor = System.Drawing.Color.Red;
                        if (ex.ErrorCode == 10061) { // Port is unused and could not establish connection 
                            L3.Text = "Результат: TCP-служба не доступна(Порт закритий)";
                            result = "port closed";
                                }
                        else {
                            L3.Text = "Результат: TCP-служба не доступна(Хост не доступний)";
                            result = "host unavalavle";
                                }
                    }
                    fuck = false;
                }
                string Query2 = $"update servise2 set check_date = '{localDate.ToString()}', result = '{result}' where ID = " + Request.QueryString["ID"];
                try
                {
                    using (SqlConnection Conn = new SqlConnection(DB))
                    {
                        Conn.Open();
                        new SqlCommand(Query2, Conn).ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if(status == "0")
            {
                L2.Text = "ВЕРИФІКАЦІЮ НЕ ПРОЙДЕНО: ПОМИЛКА ВВЕДЕННЯ ОДНОРАЗОВОГО ПАРОЛЯ!";
                L2.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                L1.Text = "По башке себе постучи!!!";
                L2.Text = "ВЕРИФІКАЦІЮ НЕ ПРОЙДЕНО: РОЗБІЙНИК НА ТЕРИТОРІЇ";
                L2.Font.Size = 100;

                L2.ForeColor = System.Drawing.Color.Red;

            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("page1.aspx");
            Button1.Visible = false;
        }
    }
}