using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace lab5_1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string Query = "select * from servise2";
            string DB = "Data Source = (LocalDB)\\MSSQLLocalDB; Initial Catalog = tcp; Integrated Security = True";

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(Query, Conn);
                SqlDataReader R = Comm.ExecuteReader();
                TableRow tr1 = new TableRow();
                TableCell td11 = new TableCell(); Label l11 = new Label();
                TableCell td22 = new TableCell(); Label l22 = new Label();
                TableCell td33 = new TableCell(); Label l33 = new Label();
                TableCell td44 = new TableCell(); Label l44 = new Label();
                TableCell td55 = new TableCell(); Label l55 = new Label();
                TableCell td66 = new TableCell(); Label l66 = new Label();

                TableCell td77 = new TableCell();

                l11.Text = "ID";            td11.Controls.Add(l11); tr1.Cells.Add(td11);
                l22.Text = "IP Address";    td22.Controls.Add(l22); tr1.Cells.Add(td22);
                l33.Text = "Port";          td33.Controls.Add(l33); tr1.Cells.Add(td33);
                l44.Text = "Logo";          td44.Controls.Add(l44); tr1.Cells.Add(td44);
                l55.Text = "Check Data";    td55.Controls.Add(l55); tr1.Cells.Add(td55);
                l66.Text = "Result";        td66.Controls.Add(l66); tr1.Cells.Add(td66);

                td11.BorderColor = Color.Red; td22.BorderColor = Color.Red;
                td33.BorderColor = Color.Red; td44.BorderColor = Color.Red;
                td55.BorderColor = Color.Red; td66.BorderColor = Color.Red;
                td77.BorderColor = Color.Red;

                td11.BorderWidth = 8; td22.BorderWidth = 8;
                td33.BorderWidth = 8; td44.BorderWidth = 8;
                td55.BorderWidth = 8; td66.BorderWidth = 8;
                td77.BorderWidth = 8;
                tr1.Cells.Add(td77);
                T1.Rows.Add(tr1);

                while (R.Read())
                {
                    TableRow tr2 = new TableRow();
                    TableCell td1 = new TableCell(); Label l1 = new Label();
                    TableCell td2 = new TableCell(); Label l2 = new Label();
                    TableCell td3 = new TableCell(); Label l3 = new Label();
                    TableCell td4 = new TableCell(); System.Web.UI.WebControls.Image l4 = new System.Web.UI.WebControls.Image();
                    TableCell td5 = new TableCell(); Label l5 = new Label();
                    TableCell td6 = new TableCell(); Label l6 = new Label();
                    TableCell td7 = new TableCell(); Button b7 = new Button();
                    l1.Text =       R[0].ToString(); td1.Controls.Add(l1); tr2.Cells.Add(td1);
                    l2.Text =       R[1].ToString(); td2.Controls.Add(l2); tr2.Cells.Add(td2);
                    l3.Text =       R[2].ToString(); td3.Controls.Add(l3); tr2.Cells.Add(td3);
                    l4.ImageUrl =   R[3].ToString(); td4.Controls.Add(l4); tr2.Cells.Add(td4); l4.Width = 50; l4.Height = 50;
                    l5.Text =       R[4].ToString(); td5.Controls.Add(l5); tr2.Cells.Add(td5);
                    l6.Text =       R[5].ToString(); td6.Controls.Add(l6); tr2.Cells.Add(td6);

                    b7.Text = "Властивості"; b7.Font.Bold = true; b7.Font.Size = 24;
                    b7.CommandArgument = l1.Text;
                    b7.Command += new CommandEventHandler(b7_Command);
                    td7.Controls.Add(b7); tr2.Cells.Add(td7);

                    td1.BorderColor = Color.Blue; td2.BorderColor = Color.Blue;
                    td3.BorderColor = Color.Blue; td4.BorderColor = Color.Blue;
                    td5.BorderColor = Color.Blue; td6.BorderColor = Color.Blue;
                    td7.BorderColor = Color.Blue;

                    td1.BorderWidth = 8; td2.BorderWidth = 8;
                    td3.BorderWidth = 8; td4.BorderWidth = 8;
                    td5.BorderWidth = 8; td6.BorderWidth = 8;
                    td7.BorderWidth = 8;
                    T1.Rows.Add(tr2);

                    T1.Font.Size = 36;
                    T1.BorderStyle = BorderStyle.Solid;
                    T1.GridLines = GridLines.Both;

                }
            }
        }
        void b7_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("page2.aspx?ID=" + HttpUtility.UrlEncode(e.CommandArgument.ToString()));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("page5.aspx");
        
        }
    }
}