using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTherapy.Web
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private List<string> Alert = new List<string>() { "Error", "Warning", "Success", "Info", "Nuovo" };
        private void renderAlerts()
        {
            if (!Alert.Any())
                return;
            //lstAlerts.Visible = true;
            string newElem = string.Empty;
            Alert.ForEach(x =>
            {
                newElem += "<li><a href=\"#\">" + x + "<span class=\"label ";
                switch (x)
                {
                    case "Error":
                        newElem += "label-danger\">Errore";
                        break;
                    case "Warning":
                        newElem += "label-warning\">Avviso";
                        break;
                    case "Success":
                        newElem += "label-success\">Messaggio";
                        break;
                    case "Info":
                        newElem += "label-info\">News";
                        break;
                    default:
                        newElem += "label-default\">Aggiunto";
                        break;
                }
                newElem += "</span></a></li>";
            });
            newElem += "<li class=\"divider\"></li><li><a href=\"#\">Vedi tutto</a></li>";
            //lstAlerts.InnerHtml = newElem;
        }
        private List<string> Errors = new List<string>();
        public void addError(string err)
        {
            Errors.Add(err);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            renderAlerts();
        }
    }
}