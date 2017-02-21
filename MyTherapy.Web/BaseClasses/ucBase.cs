using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace MyTherapy.Web
{
    public class ucBase : UserControl
    {
        protected Site SiteMaster
        {
            get
            {
                return (this.Page.Master as Site);
            }
        }
    }
}