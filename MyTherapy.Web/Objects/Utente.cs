using BL = MyTherapy.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTherapy.Web.Objects
{
    public class Utente
    {

        [SmartGrid(new SmartGridAttribute.ColumnTypeEnum[] { SmartGridAttribute.ColumnTypeEnum.DetailColumn,
                SmartGridAttribute.ColumnTypeEnum.EditColumn ,
                SmartGridAttribute.ColumnTypeEnum.DeleteColumn ,
                SmartGridAttribute.ColumnTypeEnum.CheckBox }, "", "APRI")]
        public int UserID { get; set; }
        [SmartGrid("Nome")]
        public string Nome { get; set; }
        [SmartGrid("Cognome")]
        public string Cognome { get; set; }
        [SmartGrid("Partita IVA")]
        public string PartitaIVA { get; set; }

        public Utente(BL.user user)
        {
            this.UserID = user.UserID;
            this.Nome = user.Nome;
            this.Cognome = user.Cognome;
            this.PartitaIVA = user.PartitaIVA;
        }
    }
}