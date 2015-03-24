
namespace Keystone.Web.Models.Base
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Keystone.Web.Utilities;
    public class GridDataModel
    {
        public int totalpages { get; set; }
        public int currpage { get; set; }
        public int totalrecords { get; set; }
        public IEnumerable<object> invdata { get; set; }
    }

    public class GridModelSchema
    {
        public string[] colNames { get; set; }
        public groupingView groupingView { get; set; }
        public List<colModel> colModel { get; set; }
        public jsonReader jsonReader { get; set; }

        public GridModelSchema(List<colModel> colModel, string[] colNames = null)
            : this()
        {
            this.colModel = colModel;
            this.colNames = colNames;
        }

        public GridModelSchema()
        {
            this.jsonReader = new jsonReader()
            {
                root = "invdata",
                page = "currpage",
                total = "totalpages",
                records = "totalrecords",
                repeatitems = false,
                id = "0"
            };
        }
    }

    public class colModel
    {
        public colModel()
        {
            this.title = false;
        }

        public colModel(string name)
            : this()
        {
            this.name = name;
            this.index = name;
        }

        public colModel(string name, bool editable)
            : this(name)
        {
            this.editable = editable;
            if (editable)
            {
                this.edittype = Enum.GetName(typeof(Edittype), Edittype.text).ToString();
            }
        }

        /// <summary>
        /// Gets or sets the align.
        /// </summary>
        /// <value>Applicable values [left], [center], [right].</value>
        public string align { get; set; }

        /// <summary>
        /// Gets or sets the classes.
        /// </summary>
        /// <value>This option allow to add classes to the column. If more than one class will be used a space should be set. 
        /// By example classes:'class1 class2' will set a class1 and class2 to every cell on that column. 
        /// In the grid css there is a predefined class ui-ellipsis which allow to attach ellipsis to a particular row. 
        /// Also this will work in FireFox too.</value>
        public string classes { get; set; }

        public string datefmt { get; set; }

        public bool editable { get; set; }
        public bool? internalEditable { get; set; }

        /// <summary>
        /// Gets or sets the editoptions.
        /// </summary>
        /// <value>Array of allowed options (attributes) for edittype option</value>
        public editoptions editoptions { get; set; }

        /// <summary>
        /// Gets or sets the editrules.
        /// </summary>
        /// <value>sets additional rules for the editable field</value>
        public editrules editrules { get; set; }

        public SearchOptions searchoptions { get; set; }

        public bool? search { get; set; }

        /// <summary>
        /// Gets or sets the edittype.
        /// </summary>
        /// <value>Use edittype Type. \r\n Applicable values [text], [textarea], [select], [checkbox], [password], [button], [image], [file]</value>
        public string edittype { get; set; }

        [Description("Applicable values [asc] or [desc]")]
        public string firstsortorder { get; set; }
        public bool? hidden { get; set; }
        public string index { get; set; }
        public string key { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>When colNames array is empty, defines the heading for this column. 
        /// If both the colNames array and this setting are empty, the heading for this column comes from the name property.</value>
        public string label { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>Set the unique name in the grid for the column. This property is required. 
        /// As well as other words used as property/event names, the reserved words (which cannot be used for names) include subgrid</value>
        public string name { get; set; }

        public bool? resizable { get; set; }
        public bool sortable { get; set; }
        public string sorttype { get; set; }
        public bool? title { get; set; }
        public int? width { get; set; }
        public string formatter { get; set; }

        /// <summary>
        /// Gets or sets the formatoptions.
        /// </summary>
        /// <value>Format options can be defined for particular columns, overwriting the defaults from the language file.</value>
        public formatoptions formatoptions { get; set; }

        [DefaultValue(20)]
        public int? rowNum { get; set; }
        public bool? treeGrid { get; set; }
        public bool? loadonce { get; set; }

        /// <summary>
        /// Gets or sets the tree grid model.
        /// </summary>
        /// <value>Use TreeGridModel Type. \r\n Applicable values [nested] or [adjacency]</value>
        public string treeGridModel { get; set; }

        public string treedatatype { get; set; }
        public string ExpandColumn { get; set; }
        public string caption { get; set; }
        public string targetFieldName { get; set; }
        public string summaryType { get; set; }
        public string summaryTpl { get; set; }
    }

    public class SearchOptions
    {
        public string[] sopt { get; set; }
    }

    public class formatoptions
    {
        public numberformatter number { get; set; }
        public currencyformatter currency { get; set; }

        public string decimalSeparator { get; set; }
        public string thousandsSeparator { get; set; }
        public int decimalPlaces { get; set; }
        public string defaulValue { get; set; }
        public string prefix { get; set; }
        public string suffix { get; set; }

        [Description("Applicable for [date]")]
        public string srcformat { get; set; }

        [Description("Applicable for [date]")]
        public string newformat { get; set; }

        [Description("Applicable for [link],[showlink]")]
        public string target { get; set; }

        [Description("Applicable for [showlink]")]
        public string baseLinkUrl { get; set; }

        [Description("Applicable for [showlink] \r\n showAction is an additional value which is added after the baseLinkUrl")]
        public string showAction { get; set; }

        [Description("Applicable for [showlink] \r\n addParam is an additional parameter that can be added after the idName property")]
        public string addParam { get; set; }

        [Description("Applicable for [showlink] \r\n idName is the first parameter that is added after the showAction. By default, this is id")]
        public string idName { get; set; }

        [Description("Applicable for [checkbox]")]
        public bool? disabled { get; set; }

        [Description("Applicable for [actions]")]
        public bool? keys { get; set; }

        [Description("Applicable for [actions]")]
        public bool? editbutton { get; set; }

        [Description("Applicable for [actions]")]
        public bool? delbutton { get; set; }

        [Description("Applicable for [actions]")]
        public string onEdit { get; set; }

        [Description("Applicable for [actions]")]
        public string onSuccess { get; set; }

        [Description("Applicable for [actions]")]
        public string afterSave { get; set; }

        [Description("Applicable for [actions]")]
        public string onError { get; set; }

        [Description("Applicable for [actions]")]
        public string afterRestore { get; set; }
    }

    public class numericformatter
    {
        public string decimalSeparator { get; set; }
        public string thousandsSeparator { get; set; }
        public string decimalPlaces { get; set; }
        public string defaulValue { get; set; }
    }

    public class numberformatter : numericformatter { }
    public class currencyformatter : numericformatter
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
    }

    public class editoptions
    {
        /// <summary>
        /// Gets or sets the data URL.
        /// </summary>
        /// <value>This option is valid only for elements of type select.The AJAX request is called only once when the element is created.
        /// In the inline edit or the cell edit module it is called every time when you edit a new row or cell. In the form edit module only once.</value>
        public string dataUrl { get; set; }

        /// <summary>
        /// Gets or sets the build select.
        /// </summary>
        /// <value>This option is relevant only if the dataUrl parameter is set. When the server response can not build the select element, 
        /// you can use your own function to build the select. The function should return a string containing the select and options value(s) as described in dataUrl option. 
        /// Parameter passed to this function is the server response</value>
        //[JsonConverter(typeof(AuthMDM.Web.MVC.Extensions.JsFunctionConverter))]
        public string buildSelect { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The option can be string or function. This option is valid only in Form Editing module when used with editGridRow method in add mode. 
        /// If defined the input element is set with this value if only element is empty. If used in selects the text should be provided and not the key. Also when a function is used the function should return value.</value>
        public string defaultValue { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>When set for edittype checkbox this value should be a string with two possible values separated with a colon (:) - Example editoptions:{value:“Yes:No”} where the first value determines the checked property. 
        /// When set for edittype select value can be a string, object or function.
        /// If the option is a string it must contain a set of value:label pairs with the value separated from the label with a colon (:) and ended with(;). The string should not ended with a (;)- editoptions:{value:“1:One;2:Two”}. 
        /// If set as object it should be defined as pair name:value - editoptions:{value:{1:'One';2:'Two'}} 
        /// When defined as function - the function should return either formated string or object. 
        /// In all other cases this is the value of the input element if defined.</value>
        public string value { get; set; }

        /// <summary>
        /// Gets or sets the data init.
        /// </summary>
        /// <value>The event is called only once when the element is created. 
        /// In the inline edit or the cell edit module it is called every time when you edit a new row or cell. 
        /// In the form edit module only once if the recreateForm option is set to false, or every time if the same option is set to true . </value>
        //[JsonConverter(typeof(AuthMDM.Web.MVC.Extensions.JsFunctionConverter))]
        public string dataInit { get; set; }

        /// <summary>
        /// Gets or sets the custom_element.
        /// </summary>
        /// <value>Used only if the edittype option is set to 'custom'. This function is used to create the element. 
        /// The function should return the new DOM element. Parameters passed to this function are the value and the editoptions from colModel.</value>
        //[JsonConverter(typeof(AuthMDM.Web.MVC.Extensions.JsFunctionConverter))]
        public string custom_element { get; set; }

        /// <summary>
        /// Gets or sets the custom_value.
        /// </summary>
        /// <value>Used only if the edittype option is set to 'custom'. This function should return the value from the element after the editing in order to post it to the server. 
        /// Parameter passed to this function is the element object and the operation type In inline and cell editing modules this parameters is always a string value - 'get'. 
        /// See below for the other type.
        /// 
        /// In form editing this function has a different behavior. In this case we pass additional third parameter - the value. When a values of the custom element is posted to the server the second parameter has a value 'get'. 
        /// In this case the function should return a value. If no values is returned in this case a error is raised. \
        /// In case the data is read from the grid in order to set it in the form the operation parameter has a value 'set' and the grid value is passed as a third parameter. 
        /// This way we can modify the grid value before it is displayed in the form. See the example above.</value>
        [JsonConverter(typeof(Keystone.Web.Utilities.JsFunctionConverter))]
        public string custom_value { get; set; }

        public string onchange { get; set; }
    }

    public class groupingView
    {
        //public groupingView()
        //{
        //    this.groupField = new List<string>();
        //    this.groupColumnShow = new List<string>();
        //    this.groupText = new List<string>();
        //    this.groupOrder = new List<string>();
        //    this.groupSummary = new List<string>();
        //}

        public string[] groupField { get; set; }
        public bool[] groupColumnShow { get; set; }
        public string[] groupText { get; set; }
        public bool groupCollapse { get; set; }
        public string[] groupOrder { get; set; }
        public bool[] groupSummary { get; set; }
    }

    public class editrules
    {
        public bool? edithidden { get; set; }
        public bool? required { get; set; }
        public bool? number { get; set; }
        public bool? integer { get; set; }
        public int? minValue { get; set; }
        public int? maxValue { get; set; }
        public bool? email { get; set; }
        public bool? url { get; set; }
        public bool? date { get; set; }
        public bool? time { get; set; }
        public bool? custom { get; set; }

        public string regex { get; set; }
        //[JsonConverter(typeof(AuthMDM.Web.MVC.Extensions.JsFunctionConverter))]
        public string custom_func { get; set; }
    }

    public class jsonReader
    {
        public string root { get; set; }
        public string page { get; set; }
        public string total { get; set; }
        public string records { get; set; }
        public bool? repeatitems { get; set; }
        public string id { get; set; }
    }

    public class GridSearchDataModel
    {
        public string sidx { get; set; }
        public string sord { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public bool _search { get; set; }
        public SearchFilter filters { get; set; }
    }

    public class SearchFilter
    {
        private string _groupOp;
        public string groupOp { get { return _groupOp == "AND" ? " and " : " or "; } set { _groupOp = value; } }
        public SearchRule[] rules { get; set; }

        private string ParseOperator(string opt, string data)
        {
            switch (opt)
            {
                case "eq":
                    if (data.GetCompatibleDataType() == typeof(DateTime))
                    { return "{0} = DateTime(\"{1}\")"; }
                    else if (data.GetCompatibleDataType() == typeof(string))
                    { return "{0}.EqualsSearchEx(\"{1}\")"; }
                    else
                    { return "{0} = {1}"; }
                case "ne":
                    if (data.GetCompatibleDataType() == typeof(DateTime))
                    { return "{0} <> DateTime(\"{1}\")"; }
                    else if (data.GetCompatibleDataType() == typeof(string))
                    { return "{0}.NotEqualsSearchEx(\"{1}\")"; }
                    else
                    { return "{0} <> {1}"; }
                case "le":
                    if (data.GetCompatibleDataType() == typeof(DateTime))
                        return "{0}<\"{1}\"";
                    else
                        return "{0}<{1}";
                case "lt":
                    if (data.GetCompatibleDataType() == typeof(DateTime))
                        return "{0}<=\"{1}\"";
                    else
                        return "{0}<={1}";
                case "gt":
                    if (data.GetCompatibleDataType() == typeof(DateTime))
                        return "{0}>\"{1}\"";
                    else
                        return "{0}>{1}";
                case "ge":
                    if (data.GetCompatibleDataType() == typeof(DateTime))
                        return "{0}>=\"{1}\"";
                    else
                        return "{0}>={1}";
                case "bw":
                    return "{0}.StartsWithSearchEx(\"{1}\")";
                case "ew":
                    return "{0}.EndsWithSearchEx(\"{1}\")";
                case "in":
                case "cn":
                    return "{0}.ContainsSearchEx(\"{1}\")";
                case "bn":
                    return "{0}.NotStartsWithSearchEx(\"{1}\")";
                case "en":
                    return "{0}.NotEndsWithSearchEx(\"{1}\")";
                case "ni":
                case "nc":
                    return "{0}.NotContainsSearchEx(\"{1}\")";
            }
            return string.Empty;
        }

        public override string ToString()
        {
            List<string> ruleSet = new List<string>();
            rules.ToList().ForEach(x =>
            {
                ruleSet.Add(string.Format(ParseOperator(x.op, x.data), x.field, x.data));
            });
            //if (ruleSet.Count > 0) ruleSet.Add(" StatusId = 1");
            return string.Join(this.groupOp, ruleSet);
        }
    }

    public class SearchRule
    {
        public string field { get; set; }
        public string op { get; set; }
        public string data { get; set; }
    }

    public enum Edittype
    {
        text = 1,
        textarea = 2,
        select = 3,
        checkbox = 4,
        password = 5,
        button = 6,
        image = 7,
        file = 8,
        custom = 9
    }

    public enum sorttype
    {
        integer = 1,
        number = 2,
        date = 3,
        text = 4
    }

    public enum GridMode
    {
        edit = 1,
        view = 2
    }

    public enum Formatter
    {
        integer = 1,
        number = 2,
        currency = 3,
        date = 4,
        email = 5,
        link = 6,
        showlink = 7,
        checkbox = 8,
        select = 9,
        actions = 10
    }

    public enum TreeGridModel
    {
        nested = 1,
        adjacency = 2
    }

    public enum Align
    {
        left = 1,
        center = 2,
        right = 3
    }

    public enum SummaryType
    {
        sum = 1,
        count = 2,
        avg = 3,
        min = 4,
        max = 5
    }

    public enum SearchOperator
    {
        eq = 1,
        ne = 2,
        le = 3,
        lt = 4,
        gt = 5,
        ge = 6,
        bw = 7,
        bn = 8,
        @in = 9,
        ni = 10,
        ew = 11,
        en = 12,
        cn = 13,
        nc = 14
    }
}