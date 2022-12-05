using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L1.Text = "Додавання TCP-служби";
            L1.Font.Size = 48;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string address = TextBox1.Text;
            string port = TextBox2.Text;
            string email = TextBox3.Text;

            string Query = $"insert into servise2 (address, port, photo, email) values ('{address}', {port}, '../Resources/no_foto_300.png' , '{email}')";
            string DB = "Data Source = (LocalDB)\\MSSQLLocalDB; Initial Catalog = tcp; Integrated Security = True";

            try
            {
                using (SqlConnection Conn = new SqlConnection(DB))
                {
                    Conn.Open();
                    new SqlCommand(Query, Conn).ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("page1.aspx");
        }
    }
}