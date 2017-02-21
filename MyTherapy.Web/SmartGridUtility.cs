using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MyTherapy.Web
{
    public class SmartGridAttribute : Attribute
    {
        public string Title = string.Empty;
        public string ToolTipField = string.Empty;
        public bool Visible = true;
        public List<ColumnTypeEnum> ColumnTypes = new List<ColumnTypeEnum>() { ColumnTypeEnum.Text };
        public short Order = 0;
        public string ButtonMultipleActionName = "VAI";
        public bool isDataRowKey = false;
        /// <summary>
        /// Campo utilizzato per flaggare le checkbox di scelta multipla
        /// </summary>
        public string MultipleActionDataField = string.Empty;
        /// <summary>
        /// Enum delle tipologie di colonne che possono essere renderizzate nella SmartGrid
        /// </summary>
        public enum ColumnTypeEnum
        {
            Text,
            TextNoFilter,
            Date,
            Currency,
            Decimal2Places,
            Decimal4Places,
            CheckBox,
            DetailColumn,
            EditColumn,
            DeleteColumn,
            Select
        }
        public SmartGridAttribute(string title, short order = 0, string toolTipField = "")
        {
            this.Title = title;
            this.ColumnTypes = (new ColumnTypeEnum[] { ColumnTypeEnum.Text }).ToList();
            this.Order = order;
            this.Visible = true;
            this.ToolTipField = toolTipField;
        }
        public SmartGridAttribute(string title, ColumnTypeEnum[] columnType, short order = 0, string toolTipField = "")
        {
            this.Title = title;
            this.ColumnTypes = columnType.ToList();
            this.Order = order;
            this.Visible = true;
            this.ToolTipField = toolTipField;
        }
        public SmartGridAttribute(ColumnTypeEnum[] columnType, string multipleActionDataField, string buttonMultipleActionName)
        {
            this.Title = string.Empty;
            this.ColumnTypes = columnType.ToList();
            this.MultipleActionDataField = multipleActionDataField;
            this.ButtonMultipleActionName = buttonMultipleActionName;
            this.Order = 99;
            this.Visible = false;
            this.isDataRowKey = true;
        }
    }

    #region Classes For Json
    public class StringColumn
    {
        #region Properties
        public string mDataProp { get; set; }
        public string sClass { get; set; }
        public string sWidth { get; set; }
        public string tooltip { get; set; }
        public bool orderable { get; set; }
        #endregion

        #region Constructors
        public StringColumn(SmartGridColumnInfo info, int swidth, SmartGridAttribute.ColumnTypeEnum colType)
        {
            string mdataprop = info.PropertyName;
            sClass = "c" + mdataprop;
            switch (colType)
            {
                case SmartGridAttribute.ColumnTypeEnum.Text:
                case SmartGridAttribute.ColumnTypeEnum.TextNoFilter:
                case SmartGridAttribute.ColumnTypeEnum.Date:
                case SmartGridAttribute.ColumnTypeEnum.Currency:
                case SmartGridAttribute.ColumnTypeEnum.Decimal2Places:
                case SmartGridAttribute.ColumnTypeEnum.Decimal4Places:
                case SmartGridAttribute.ColumnTypeEnum.Select:
                case SmartGridAttribute.ColumnTypeEnum.CheckBox:
                    mDataProp = mdataprop;
                    orderable = true;
                    if (!string.IsNullOrEmpty(info.Attribute.ToolTipField))
                        sClass = info.Attribute.ToolTipField;
                    sWidth = swidth + "%";
                    break;
            }
        }
        public StringColumn(SmartGridColumnInfo info, int swidth, SmartGridAttribute.ColumnTypeEnum colType, bool orderable)
        {
            string mdataprop = info.PropertyName;
            sClass = "c" + mdataprop;
            switch (colType)
            {
                case SmartGridAttribute.ColumnTypeEnum.Text:
                case SmartGridAttribute.ColumnTypeEnum.TextNoFilter:
                case SmartGridAttribute.ColumnTypeEnum.Date:
                case SmartGridAttribute.ColumnTypeEnum.Currency:
                case SmartGridAttribute.ColumnTypeEnum.Decimal2Places:
                case SmartGridAttribute.ColumnTypeEnum.Decimal4Places:
                case SmartGridAttribute.ColumnTypeEnum.Select:
                case SmartGridAttribute.ColumnTypeEnum.CheckBox:
                    mDataProp = mdataprop;
                    this.orderable = orderable;
                    if (!string.IsNullOrEmpty(info.Attribute.ToolTipField))
                        sClass = info.Attribute.ToolTipField;
                    sWidth = swidth + "%";
                    break;
            }
        }
        #endregion
    }
    public class CustomColumn
    {
        #region Properties
        public string mDataProp { get; set; }
        public string className { get; set; }
        public bool orderable { get; set; }
        public string fnCreatedCell { get; set; }
        public string mRender { get; set; }
        public string sWidth { get; set; }
        #endregion

        public CustomColumn(SmartGridColumnInfo info, SmartGridAttribute.ColumnTypeEnum colType)
        {
            string mdataprop = info.PropertyName;
            switch (colType)
            {
                case SmartGridAttribute.ColumnTypeEnum.DetailColumn:
                case SmartGridAttribute.ColumnTypeEnum.EditColumn:
                case SmartGridAttribute.ColumnTypeEnum.DeleteColumn:
                    className = colType.ToString();
                    mDataProp = mdataprop;
                    orderable = false;
                    sWidth = "0%";
                    break;
            }
        }
    }
    public class StringColumnDef
    {
        public string sType { get; set; }
        public StringColumnDef()
        {
            sType = "string";
        }
    }
    public class CostumColumnDef
    {
        #region Properties
        public int targets { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public string width { get; set; }
        public string render { get; set; }
        #endregion

        public CostumColumnDef(SmartGridColumnInfo info, int index, SmartGridAttribute.ColumnTypeEnum colType)
        {
            switch (colType)
            {
                case SmartGridAttribute.ColumnTypeEnum.CheckBox:
                    targets = index;
                    searchable = false;
                    orderable = false;
                    width = "1%";
                    render = colType.ToString();
                    break;
                case SmartGridAttribute.ColumnTypeEnum.Date:
                case SmartGridAttribute.ColumnTypeEnum.Currency:
                case SmartGridAttribute.ColumnTypeEnum.Decimal2Places:
                case SmartGridAttribute.ColumnTypeEnum.Decimal4Places:
                    targets = index;
                    searchable = false;
                    orderable = true;
                    render = colType.ToString();
                    break;
            }
        }
    }
    #endregion

    /// <summary>
    /// Classe che descrive la struttura di ogni proprietà della struttura dati che verrà visualizzata nella SmartGrid
    /// </summary>
    public class SmartGridColumnInfo
    {
        #region Properties
        public PropertyInfo PropInfo;
        /// <summary>
        /// SmartGridAttribute legato alla Property Info
        /// </summary>
        public SmartGridAttribute Attribute { get; set; }
        #endregion
        /// <summary>
        /// Proprietà che ritorna il "ColumnSpan" della colonna
        /// </summary>
        public int RenderedColumnsCount
        {
            get
            {
                if (Attribute == null)
                    return -1;
                return Attribute.ColumnTypes
                    .Count(x => x == SmartGridAttribute.ColumnTypeEnum.DetailColumn
                        || x == SmartGridAttribute.ColumnTypeEnum.EditColumn
                        || x == SmartGridAttribute.ColumnTypeEnum.DeleteColumn
                        || x == SmartGridAttribute.ColumnTypeEnum.CheckBox)
                    + (Attribute.Visible ? 1 : 0);
            }
        }
        /// <summary>
        /// Proprietà che ritorna il nome della Propery Info(DataField)
        /// </summary>
        public string PropertyName
        {
            get
            {
                if (PropInfo != null)
                    return (PropInfo.Name + "").Trim();
                return string.Empty;
            }
        }

        #region Constructors
        /// <summary>
        /// Costruttore utilizzato per le entità
        /// </summary>
        public SmartGridColumnInfo(PropertyInfo propInfo, SmartGridAttribute smartGridAttr)
        {
            PropInfo = propInfo;
            Attribute = smartGridAttr;
        }
        #endregion
    }
    public class SmartGridEventHandler
    {
        public delegate void EventDelegate(string id);
        public EventDelegate DetailEventDelegate { get; set; }
        public EventDelegate EditEventDelegate { get; set; }
        public EventDelegate DeleteEventDelegate { get; set; }
        public EventDelegate MultipleActionEventDelegate { get; set; }

        public SmartGridEventHandler(EventDelegate eventDetail, EventDelegate eventEdit, EventDelegate eventDelete, EventDelegate eventMultipleAction)
        {
            DetailEventDelegate = eventDetail;
            EditEventDelegate = eventEdit;
            DeleteEventDelegate = eventDelete;
            MultipleActionEventDelegate = eventMultipleAction;
        }
    }
}