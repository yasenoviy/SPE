using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        string address, port, email, fn;
        protected void Page_Load(object sender, EventArgs e)
        {
            L1.Text = "Властивості TCP-служби з ID=" + Request.QueryString["ID"];
            L1.Font.Size = 48;

            string Query = "select * from servise2 where ID = " + Request.QueryString["ID"];
            string DB = "Data Source = (LocalDB)\\MSSQLLocalDB; Initial Catalog = tcp; Integrated Security = True";

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(Query, Conn);
                SqlDataReader R = Comm.ExecuteReader();

                Label1.Text = "IP";
                Label2.Text = "Port";
                Label3.Text = "Logo";
                Label4.Text = "Upload new logo";
                Label5.Text = "Check data";
                Label6.Text = "Result";
                Label7.Text = "Email";

                //test
                //Label10.Text = l66.Text;
                //test

                address = l11.Text;
                port = l22.Text;
                email = l66.Text;

                //load from MSSQl
                while (R.Read())
                {

                    l11.Text = R[1].ToString();
                    l22.Text = R[2].ToString();
                    l33.ImageUrl = R[3].ToString(); l33.Width = 100; l33.Height = 100;
                    l44.Text = R[4].ToString();
                    l55.Text = R[5].ToString();
                    l66.Text = R[6].ToString(); 
                }

                //use unsaved data from textbox
                if(address != "")
                {
                    l11.Text = address;
                }
                if(port != "")
                {
                    l22.Text = port;
                }
                if(email != "")
                {
                    l66.Text = email;
                }


            }
        }


        protected void l11_TextChanged(object sender, EventArgs e) { }

        protected void l22_TextChanged(object sender, EventArgs e) { }


        protected void l66_TextChanged(object sender, EventArgs e) { }

        
        //file upload
        protected void Button1_Click(object sender, EventArgs e)
        {
            if ((F1.PostedFile != null) && (F1.PostedFile.ContentLength > 0))
            {

                fn = System.IO.Path.GetFileName(F1.PostedFile.FileName);
                string SaveLocation = Server.MapPath("Resources") + "\\" + fn;
                if (fn.Contains(".jpg") || fn.Contains(".jpeg") || fn.Contains(".png"))
                {
                    try
                    {
                        F1.PostedFile.SaveAs(SaveLocation);
                        FileUploadStatus.Text = "The file has been uploaded.";
                        try
                        {
                            string Query2 = $"update servise2 set photo = '../Resources/{fn}' where ID = " + Request.QueryString["ID"];
                            string DB = "Data Source = (LocalDB)\\MSSQLLocalDB; Initial Catalog = tcp; Integrated Security = True";

                            using (SqlConnection Conn = new SqlConnection(DB))
                            {
                                Conn.Open();
                                new SqlCommand(Query2, Conn).ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            FileUploadStatus.Text = ex.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        FileUploadStatus.Text = "Error: " + ex.Message;
                    }
                }
                else
                {
                    FileUploadStatus.Text = "Choose photo with format .png, .jpeg or .jpg";
                    fn = "";
                }
            }
            else
            {
                FileUploadStatus.Text = "Please select a file to upload.";
            }
        }
 
        //return button
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("page1.aspx");
        }

        //save button
        protected void Button2_Click(object sender, EventArgs e)
        {

            string Query = $"update servise2 set address = '{l11.Text}', port = '{l22.Text}', email = '{l66.Text}'  where ID = " + Request.QueryString["ID"];
            string DB = "Data Source = (LocalDB)\\MSSQLLocalDB; Initial Catalog = tcp; Integrated Security = True";
            try
            {
                using (SqlConnection Conn = new SqlConnection(DB))
                {
                    Conn.Open();
                    new SqlCommand(Query, Conn).ExecuteNonQuery();
                }
                Label10.Text = Query;
            }
            catch (Exception ex)
            {
                Label10.Text = ex.Message;
            }

            System.Threading.Thread.Sleep(3000);
            Response.Redirect("page1.aspx");


        }

        //test button
        protected void Button4_Click(object sender, EventArgs e)
        {
            Response.Redirect("page3.aspx?ID=" + Request.QueryString["ID"]);
        }
    }
}