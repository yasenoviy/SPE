using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        string temp, temp1;
        bool fuck = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
            {
                L1.Text = "Перевірка TCP-служби з ID=" + Request.QueryString["ID"];
                L1.Font.Size = 48;

                if (fuck)
                {
                    string Query = "select email from servise2 where ID = " + Request.QueryString["ID"];
                    string DB = "Data Source = (LocalDB)\\MSSQLLocalDB; Initial Catalog = tcp; Integrated Security = True";
                    string email = "";
                    temp = Label11.Text;

                    using (SqlConnection Conn = new SqlConnection(DB))
                    {
                        Conn.Open();
                        SqlCommand Comm = new SqlCommand(Query, Conn);
                        SqlDataReader R = Comm.ExecuteReader();
                        while (R.Read())
                        {
                            email = R[0].ToString();
                        }

                        string autentifer = GetRandomString(5);
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.Credentials = new NetworkCredential("ayasenoviy@knu.ua", "nbeiqciezdetrszv");
                        client.EnableSsl = true;
                        client.Send("ayasenoviy@knu.ua", email, "Confirm your password!", autentifer);

                        Label11.Text = autentifer;
                        fuck = false;
                    }
                }


            }
            else
            {
                L1.Text = "Вийди звідси, розбійник";
                L1.Font.Size = 48;
                TextBox1.Visible = false;
            }
            


        }


        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            temp1 = temp;
            Label12.Text = temp1;
        }
        private string GetRandomString(int length)
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                string apended = ((char)('a' + random.Next(0, 25))).ToString();
                builder.Append(random.Next(0, 2) == 0 ? apended.ToUpper() : apended);
            }
            return builder.ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string status;
            if (TextBox1.Text == temp)
            {
                status = "1";
                //Label13.Text = TextBox1.Text + "....." + temp;
            }
            else
            {
                status = "0";
                //Label13.Text = TextBox1.Text + "....." + temp;

            }
            //Label11.Text += "     " + status;
            Response.Redirect("page4.aspx?ID=" + Request.QueryString["ID"] + "&status=" + status);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("page1.aspx");
        }
    }
}