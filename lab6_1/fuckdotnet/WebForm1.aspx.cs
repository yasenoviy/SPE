using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace fuckdotnet
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            double x = Convert.ToInt32(TextBox1.Text);
            double y = Convert.ToInt32(TextBox2.Text);

            double res = Library_4.KI3_Class_4.F4(x, y);

            Label4.Text = res.ToString();
        }
    }
}