using BL = MyTherapy.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTherapy.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucUtenti.LoadSmartGrid(BL.Presenter.DefaultPresenter.GetUsers().Select(x => new Objects.Utente(x)).ToList(),
                false, false, new List<ucSmartGrid.ButtonTypeEnum>() { ucSmartGrid.ButtonTypeEnum.Excel, ucSmartGrid.ButtonTypeEnum.Pdf, ucSmartGrid.ButtonTypeEnum.Print },
                "Utenti");
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ucSmartGrid.SmartGridData GetObjects()
        {
            return ucSmartGrid.GetObjects();
        }
    }
}