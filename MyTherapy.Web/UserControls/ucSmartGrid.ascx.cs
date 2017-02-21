using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Text;
using System.Web;

namespace MyTherapy.Web
{
    public partial class ucSmartGrid : ucBase
    {
        #region Properties
        private List<SmartGridColumnInfo> _ListSmartGridColumnInfo;
        /// <summary>
        /// Lista che contiene le informazioni relative le colonne della SmartGrid
        /// </summary>
        private List<SmartGridColumnInfo> ListSmartGridColumnInfo
        {
            get { return (_ListSmartGridColumnInfo != null ? _ListSmartGridColumnInfo.Where(x => x.Attribute != null).ToList() : new List<SmartGridColumnInfo>()); }
            set { _ListSmartGridColumnInfo = value; }
        }
        /// <summary>
        /// Nome della pagina utilizzato per richiamare il WebMethod che recupera i dati
        /// </summary>
        public string PageName
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
            }
        }
        /// <summary>
        /// ClientId della smartGrid
        /// </summary>
        public string TableName = "tblSmartGrid";
        public string FileName;
        public string sDom = "<'row container-fluid'<'col-sm-1'{0}><'col-sm-3'{1}><'col-sm-4 dataTables_selectedRows'><'col-sm-3'{2}><'col-sm-1 dataTables_clearFilters'>> <'row'<'col-sm-12'rt>> <'row'<'col-sm-5'i><'col-sm-1 dataTables_toggleCheckboxes'><'col-sm-6'p>>";

        /// <summary>
        /// Enum delle tipologie di bottoni
        /// </summary>
        public enum ButtonTypeEnum
        {
            ColumnVisibility,
            Excel,
            Pdf,
            Print,
        }
        /// <summary>
        /// DataField che viene utilizzato come chiave nell'Event Handler
        /// </summary>
        public string DataRowKey
        {
            get
            {
                if (ViewState["DataRowKey"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["DataRowKey"];
            }
            set { ViewState["DataRowKey"] = value; }
        }
        /// <summary>
        /// DataField che indica se la checkBox è selezionata
        /// </summary>
        public string MultipleActionDataField
        {
            get
            {
                if (ViewState["MultipleActionDataField"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["MultipleActionDataField"];
            }
            set { ViewState["MultipleActionDataField"] = value; }
        }
        /// <summary>
        /// Valore booleano che indica se la checkBox è selezionata quando il "MultipleActionDataField" è TRUE o FALSE
        /// </summary>
        public bool MultipleActionDataValue
        {
            get
            {
                if (ViewState["MultipleActionDataValue"] == null)
                    return true;
                else
                    return ViewState["MultipleActionDataValue"].ToString().ToLower() == "true";
            }
            set { ViewState["MultipleActionDataValue"] = value.ToString().ToLower(); }
        }
        /// <summary>
        /// Istanza della classe Event Handler
        /// </summary>
        private SmartGridEventHandler EventHandler;
        #endregion

        #region Custom Column Class
        /// <summary>
        /// Classe utilizzata lato client per la gestione dell'evento Detai
        /// </summary>
        public string DetailColumnClass
        {
            get
            {
                return SmartGridAttribute.ColumnTypeEnum.DetailColumn.ToString();
            }
        }
        /// <summary>
        /// Classe utilizzata lato client per la gestione dell'evento Edit
        /// </summary>
        public string EditColumnClass
        {
            get
            {
                return SmartGridAttribute.ColumnTypeEnum.EditColumn.ToString();
            }
        }
        /// <summary>
        /// Classe utilizzata lato client per la gestione dell'evento Delete
        /// </summary>
        public string DeleteColumnClass
        {
            get
            {
                return SmartGridAttribute.ColumnTypeEnum.DeleteColumn.ToString();
            }
        }
        /// <summary>
        /// Classe utilizzata lato client per la gestione dell'evento MultipleAction
        /// </summary>
        public string MultipleActionColumnClass
        {
            get
            {
                return SmartGridAttribute.ColumnTypeEnum.CheckBox.ToString();
            }
        }
        #endregion

        #region Action Key
        /// <summary>
        /// Enum che viene utilizzata per gli eventi lato Client
        /// </summary>
        public enum ActionKeyEnum
        {
            Detail,
            Edit,
            Delete,
            MultipleAction
        }
        public string DetailKey
        {
            get
            {
                return ActionKeyEnum.Detail.ToString();
            }
        }
        public string EditKey
        {
            get
            {
                return ActionKeyEnum.Edit.ToString();
            }
        }
        public string DeleteKey
        {
            get
            {
                return ActionKeyEnum.Delete.ToString();
            }
        }
        public string MultipleActionKey
        {
            get
            {
                return ActionKeyEnum.MultipleAction.ToString();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Gestisce le operazioni di postBack scatenate lato Client
        /// </summary>
        /// <param name="EventArgs">Formato da 3 stringhe suddivise da '_', indica l'operazione e la chiave per l'evento</param>
        private void GestisciPostBack(string EventArgs)
        {
            if (EventArgs.StartsWith("Operation"))
            {
                string[] arguments = EventArgs.Split(new char[] { '_' });

                if (arguments.Length != 3)
                {
                    //errore
                    return;
                }
                switch ((ActionKeyEnum)Enum.Parse(typeof(ActionKeyEnum), arguments[1]))
                {
                    case ActionKeyEnum.Detail:
                        OpenDetail(arguments[2]);
                        break;
                    case ActionKeyEnum.Edit:
                        OpenEdit(arguments[2]);
                        break;
                    case ActionKeyEnum.Delete:
                        OpenDelete(arguments[2]);
                        break;
                    case ActionKeyEnum.MultipleAction:
                        OpenMultipleAction(arguments[2]);
                        break;
                }
            }
        }
        /// <summary>
        /// Metodo che richiamo il delegato Detail
        /// </summary>
        private void OpenDetail(string id)
        {
            if (EventHandler != null && EventHandler.DetailEventDelegate != null)
                EventHandler.DetailEventDelegate(id);
        }
        /// <summary>
        /// Metodo che richiamo il delegato Edit
        /// </summary>
        private void OpenEdit(string id)
        {
            if (EventHandler != null && EventHandler.EditEventDelegate != null)
                EventHandler.EditEventDelegate(id);
        }
        /// <summary>
        /// Metodo che richiamo il delegato Delete
        /// </summary>
        private void OpenDelete(string id)
        {
            if (EventHandler != null && EventHandler.DeleteEventDelegate != null)
                EventHandler.DeleteEventDelegate(id);
        }
        /// <summary>
        /// Metodo che richiamo il delegato MultipleAction
        /// </summary>
        private void OpenMultipleAction(string id)
        {
            if (EventHandler != null && EventHandler.MultipleActionEventDelegate != null)
                EventHandler.MultipleActionEventDelegate(id);
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
            GestisciPostBack((Request["__EVENTARGUMENT"] + "").Trim());

            if (IsPostBack) return;
        }
        /// <summary>
        /// Caricamento standard della SmartGrid
        /// </summary>
        /// <param name="objects">DataSource della SmartGrid</param>
        /// <param name="lengthMenu">Permette la selezione del numero di righe della SmartGrid</param>
        /// <param name="horizontalSearch">Abilita la ricerca orizzontale</param>
        public void LoadSmartGrid<T>(List<T> objects, bool lengthMenu = true, bool horizontalSearch = false, List<ButtonTypeEnum> btns = null, string fileName = "Data")
        {
            FileName = fileName + "_" + DateTime.Now.ToString("yyyyMMdd");
            SetDataSource<T>(objects, lengthMenu, horizontalSearch, btns);
        }
        /// <summary>
        /// Inizializzazione dell'Event Handler
        /// </summary>
        /// <param name="openDetail">Delegato per l'evento Detail</param>
        /// <param name="openEdit">Delegato per l'evento Edit</param>
        /// <param name="openDelete">Delegato per l'evento Delete</param>
        /// <param name="openMultipleAction">Delegato per l'evento MultipleAction</param>
        public void SetHandlers(SmartGridEventHandler.EventDelegate openDetail = null,
            SmartGridEventHandler.EventDelegate openEdit = null, SmartGridEventHandler.EventDelegate openDelete = null,
            SmartGridEventHandler.EventDelegate openMultipleAction = null)
        {
            this.EventHandler = new SmartGridEventHandler(openDetail, openEdit, openDelete, openMultipleAction);
        }
        /// <summary>
        /// Metodo che inizializza lo UserControl con gli elementi base della SmartGrid e salva in sessione i dati necessari alla renderizzazione della SmartGrid
        /// </summary>
        /// <param name="objects">DataSource della SmartGrid</param>
        /// <param name="lengthMenu">Permette la selezione del numero di righe della SmartGrid</param>
        /// <param name="horizontalSearch">Abilita la ricerca orizzontale</param>
        private void SetDataSource<T>(List<T> objects, bool lengthMenu, bool horizontalSearch, List<ButtonTypeEnum> btns)
        {
            try
            {
                List<object> Objects = objects.Cast<object>().ToList();
                GetProperyInfo<T>(objects);

                if (ListSmartGridColumnInfo == null || ListSmartGridColumnInfo.Count == 0)
                    return;
                CreateTable();

                int width = 100 / ListSmartGridColumnInfo
                    .Count(x => x.Attribute.Visible);
                List<object> Columns = new List<object>(),
                    ColumnsDefs = new List<object>();
                int index = 0;
                ListSmartGridColumnInfo
                    .ToList()
                    .ForEach(x =>
                     {
                         if (x.Attribute.isDataRowKey)
                         {
                             DataRowKey = x.PropertyName;
                             MultipleActionDataValue = !x.Attribute.MultipleActionDataField.StartsWith("!");
                             MultipleActionDataField = x.Attribute.MultipleActionDataField.Substring(MultipleActionDataValue ? 0 : 1);
                         }
                         x.Attribute.ColumnTypes.ForEach(
                             y =>
                             {
                                 object col, colDef;
                                 switch (y)
                                 {
                                     case SmartGridAttribute.ColumnTypeEnum.DetailColumn:
                                     case SmartGridAttribute.ColumnTypeEnum.EditColumn:
                                     case SmartGridAttribute.ColumnTypeEnum.DeleteColumn:
                                         col = new CustomColumn(x, y);
                                         colDef = new StringColumnDef();
                                         break;
                                     case SmartGridAttribute.ColumnTypeEnum.CheckBox:
                                     case SmartGridAttribute.ColumnTypeEnum.Date:
                                         col = new StringColumn(x, width, y, false);
                                         colDef = new CostumColumnDef(x, index, y);
                                         break;
                                     case SmartGridAttribute.ColumnTypeEnum.Currency:
                                     case SmartGridAttribute.ColumnTypeEnum.Decimal2Places:
                                     case SmartGridAttribute.ColumnTypeEnum.Decimal4Places:
                                         col = new StringColumn(x, width, y, true);
                                         colDef = new CostumColumnDef(x, index, y);
                                         break;
                                     default:
                                         col = new StringColumn(x, width, y);
                                         colDef = new StringColumnDef();
                                         break;
                                 }
                                 Columns.Add(col);
                                 ColumnsDefs.Add(colDef);
                                 index++;
                             });
                     });

                SmartGridData smartGridData = new SmartGridData();
                var jsonSerialiser = new JavaScriptSerializer();

                bool btnFeatures = false;
                if (btns != null && btns.Count > 0)
                {
                    List<string> renderedButtons = new List<string>();
                    btns.ForEach(x => { renderedButtons.Add(x.ToString()); });
                    btnFeatures = true;
                    smartGridData.Buttons = jsonSerialiser.Serialize(renderedButtons);
                }
                smartGridData.sDom = string.Format(sDom, lengthMenu ? "l" : "", btnFeatures ? "B" : "", horizontalSearch ? "f" : "");
                smartGridData.Objects = jsonSerialiser.Serialize(Objects);
                smartGridData.Columns = jsonSerialiser.Serialize(Columns);
                smartGridData.ColumnsDefs = jsonSerialiser.Serialize(ColumnsDefs);

                System.Web.HttpContext.Current.Session.Add("SmartGridData", smartGridData);
            }
            catch (Exception /*ex*/)
            {
                //Logger.LogException(ex);
                SiteMaster.addError("Errore in fase di caricamento della SmartGrid");
            }
        }
        /// <summary>
        /// Metodo per la creazione della tabella sulla quale verrà costruita la SmartGrid
        /// </summary>
        private void CreateTable()
        {
            StringBuilder sb = new StringBuilder();

            #region Create Table
            sb.AppendLine("<table id=\"" + TableName + "\" class=\"table table-striped display select\" style=\"cursor: pointer\">");

            sb.AppendLine("<thead>");
            #region Header
            ListSmartGridColumnInfo.ForEach(x =>
            {
                x.Attribute.ColumnTypes.ForEach(
                    y =>
                    {
                        switch (y)
                        {
                            case SmartGridAttribute.ColumnTypeEnum.Text:
                            case SmartGridAttribute.ColumnTypeEnum.TextNoFilter:
                            case SmartGridAttribute.ColumnTypeEnum.Date:
                            case SmartGridAttribute.ColumnTypeEnum.Currency:
                            case SmartGridAttribute.ColumnTypeEnum.Decimal2Places:
                            case SmartGridAttribute.ColumnTypeEnum.Decimal4Places:
                            case SmartGridAttribute.ColumnTypeEnum.Select:
                                sb.AppendLine("<th>" + x.Attribute.Title + "</th>");
                                break;
                            case SmartGridAttribute.ColumnTypeEnum.CheckBox:
                                sb.AppendLine("<th class=\"text-center\"><input id='select_all' title='Seleziona tutto' type='checkbox' /><span class=\"fakeCheck\" onclick=\"fakeCheckClick(this)\"></span></th>");
                                break;
                            default:
                                sb.AppendLine("<th></th>");
                                break;
                        }
                    });
            });
            #endregion

            sb.AppendLine("</thead>");

            sb.AppendLine("<tfoot class=\"filterRow\" >");
            #region Footer
            ListSmartGridColumnInfo.ForEach(x =>
            {
                x.Attribute.ColumnTypes.ForEach(
                    y =>
                    {
                        switch (y)
                        {
                            case SmartGridAttribute.ColumnTypeEnum.Text:
                                sb.AppendLine("<th data-title=\"" + x.Attribute.Title + "\"></th>");
                                break;
                            case SmartGridAttribute.ColumnTypeEnum.Select:
                                sb.AppendLine("<th class=\"IncludiFiltroSelect\"></th>");
                                break;
                            case SmartGridAttribute.ColumnTypeEnum.CheckBox:
                                sb.AppendLine("<th class=\"EscludiFiltro text-center\"><input id='btnMultipleAction' type='button' class='btn btn-success' value='" + x.Attribute.ButtonMultipleActionName + "' /></th>");
                                break;
                            default:
                                sb.AppendLine("<th class=\"EscludiFiltro\"></th>");
                                break;
                        }
                    });
            });
            #endregion
            sb.AppendLine("</tfoot>");

            sb.AppendLine("</table>");
            #endregion

            divSmartGrid.InnerHtml = sb.ToString();
        }
        /// <summary>
        /// Metodo che estrae la lista delle SmartGridColumnInfo per la definizione delle colonne della SmartGrid
        /// </summary>
        /// <param name="Objects">Lista degli oggetti da visualizzare nella SmartGrid</param>
        private void GetProperyInfo<T>(List<T> Objects)
        {
            Type type = Objects.FirstOrDefault<T>().GetType();

            ListSmartGridColumnInfo = GetSmartGridAttribute<T>(Objects.FirstOrDefault<T>()).OrderBy(x => (x.Attribute != null ? x.Attribute.Order : -1)).ToList();
        }
        private List<SmartGridColumnInfo> GetSmartGridAttribute<T>(T obj)
        {
            List<SmartGridColumnInfo> allProperties = obj.GetType()
               .GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name != "ExtensionData")
               .Select(x => new SmartGridColumnInfo(x,
                (SmartGridAttribute)Attribute.GetCustomAttribute(x, typeof(SmartGridAttribute), true)))
               .ToList();
            return allProperties;
        }

        #region Ajax
        /// <summary>
        /// Metodo statico richiamato dal WebMethod della pagina
        /// </summary>
        public static SmartGridData GetObjects()
        {
            return (SmartGridData)System.Web.HttpContext.Current.Session["SmartGridData"];
        }
        [Serializable]
        public class SmartGridData
        {
            /// <summary>
            /// sDom della SmartGrid
            /// </summary>
            public string sDom { get; set; }
            /// <summary>
            /// Buttons della SmartGrid
            /// </summary>
            public string Buttons { get; set; }
            /// <summary>
            /// Json contenente la lista degli oggetti da visualizzare nella SmartGrid
            /// </summary>
            public string Objects { get; set; }
            /// <summary>
            /// Json contenente la struttura Columns di datatables.net
            /// </summary>
            public string Columns { get; set; }
            /// <summary>
            /// Json contenente la struttura ColumnsDefs di datatables.net
            /// </summary>
            public string ColumnsDefs { get; set; }
        }
        #endregion
    }

}
