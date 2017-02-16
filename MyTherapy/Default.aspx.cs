using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTherapy
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void clickMeButton_Click(object sender, EventArgs e)
        {
            object val = ViewState["ButtonClickCount"];
            int i = (val == null) ? 1 : (int)val + 1;
            outputlabel.Text = string.Format("You clicked me {0} {1}", i, i == 1 ? "time" : "times");
            ViewState["ButtonClickCount"] = i;
        }
    }
}