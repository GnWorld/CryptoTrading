

namespace Admin.NET.Core.Service;

/// <summary>
/// 自定义模板引擎
/// </summary>
public class CustomViewEngine : ViewEngineModel
{
    /// <summary>
    /// 库定位器
    /// </summary>
    public string ConfigId { get; set; } = SqlSugarConst.MainConfigId;

    public string AuthorName { get; set; }

    public string BusName { get; set; }

    public string NameSpace { get; set; }

    public string ClassName { get; set; }

    public string LowerClassName { get; set; }

    public string ProjectLastName { get; set; }

    public string PagePath { get; set; } = "main";

    public string PrintType { get; set; }

    public string PrintName { get; set; }

    public bool HasLikeQuery { get; set; }

    public bool HasJoinTable { get; set; }

    public bool HasEnumField { get; set; }

    public bool HasDictField { get; set; }

    public bool HasConstField { get; set; }

    public bool HasSetStatus => TableField.Any(IsStatus);

    public List<CodeGenConfig> TableField { get; set; }

    public List<CodeGenConfig> ImportFieldList { get; set; }

    public List<CodeGenConfig> UploadFieldList { get; set; }

    public List<CodeGenConfig> QueryWhetherList { get; set; }

    public List<CodeGenConfig> ApiTreeFieldList { get; set; }

    public List<CodeGenConfig> DropdownFieldList { get; set; }

    public List<CodeGenConfig> AddUpdateFieldList { get; set; }

    public List<CodeGenConfig> PrimaryKeyFieldList { get; set; }

    public List<TableUniqueConfigItem> TableUniqueConfigList { get; set; }

    public List<CodeGenConfig> IgnoreUpdateFieldList => TableField.Where(u => u.WhetherAddUpdate == "N" && u.ColumnKey != "True" && u.WhetherCommon != "Y").ToList();

    /// <summary>
    /// 格式化主键查询条件
    /// 例： PrimaryKeysFormat(" || ", "u.{0} == input.{0}")
    /// 单主键返回 u.Id == input.Id
    /// 组合主键返回 u.Id == input.Id || u.FkId == input.FkId
    /// </summary>
    /// <param name="separator">分隔符</param>
    /// <param name="format">模板字符串</param>
    /// <param name="lowerFirstLetter">字段首字母小写</param>
    /// <returns></returns>
    public string PrimaryKeysFormat(string separator, string format, bool lowerFirstLetter = false) => string.Join(separator, PrimaryKeyFieldList.Select(u => string.Format(format, lowerFirstLetter ? u.LowerPropertyName : u.PropertyName)));

    /// <summary>
    /// 注入的服务
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> InjectServiceMap
    {
        get
        {
            var injectMap = new Dictionary<string, string>();
            if (UploadFieldList.Count > 0) injectMap.Add(nameof(SysFileService), ToLowerFirstLetter(nameof(SysFileService)));
            if (DropdownFieldList.Count > 0 || ImportFieldList.Count > 0) injectMap.Add(nameof(ISqlSugarClient), ToLowerFirstLetter(nameof(ISqlSugarClient).TrimStart('I')));
            if (ImportFieldList.Any(c => c.EffectType == "DictSelector")) injectMap.Add(nameof(SysDictTypeService), ToLowerFirstLetter(nameof(SysDictTypeService)));
            return injectMap;
        }
    }

    /// <summary>
    /// 服务构造参数
    /// </summary>
    public string InjectServiceArgs => InjectServiceMap.Count > 0 ? ", " + string.Join(", ", InjectServiceMap.Select(kv => $"{kv.Key} {kv.Value}")) : "";

    /// <summary>
    /// 默认值列表
    /// </summary>
    public List<CodeGenConfig> DefaultValueList { get; set; }

    /// <summary>
    /// 判断字段是否为状态字段
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns> 
    public bool IsStatus(CodeGenConfig column) => column.NetType == nameof(StatusEnum);

    /// <summary>
    /// 获取首字母小写字符串
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string ToLowerFirstLetter(string text) => string.IsNullOrWhiteSpace(text) ? text : text[..1].ToLower() + text[1..];

    /// <summary>
    /// 将基本字段类型转为可空类型
    /// </summary>
    /// <param name="netType"></param>
    /// <returns></returns>
    public string GetNullableNetType(string netType) => Regex.IsMatch(netType, "(.*?Enum|bool|char|int|long|double|float|decimal)[?]?") ? netType.TrimEnd('?') + "?" : netType;

    /// <summary>
    /// 获取前端表格列定义的属性
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetElTableColumnCustomProperty(CodeGenConfig column)
    {
        var content = $"prop='{column.LowerPropertyName}' label='{column.ColumnComment}'";
        if (IsStatus(column)) content += $" v-auth=\"'{LowerClassName}:setStatus'\"";
        if (column.WhetherSortable == "Y") content += " sortable='custom'";
        return content;
    }

    /// <summary>
    /// 设置默认值
    /// </summary>
    /// <returns></returns>
    public string GetAddDefaultValue()
    {
        var content = "";
        if (DefaultValueList.Count == 0)
        {
            var status = TableField.FirstOrDefault(IsStatus);
            var orderNo = TableField.FirstOrDefault(c => c.NetType.TrimEnd('?') == "int" && c.PropertyName == nameof(SysUser.OrderNo));
            if (status != null) content += $"{status.LowerPropertyName}: {(int)StatusEnum.Enable},";
            if (orderNo != null) content += $"{orderNo.LowerPropertyName}: 100,";
        }
        else
        {
            foreach (var item in DefaultValueList)
            {
                if (!string.IsNullOrWhiteSpace(item.DefaultValue))
                {
                    switch (item.EffectType)
                    {
                        case "InputNumber":
                        case "EnumSelector"://枚举和数字框，通过正则提取 数字：如 ('0')
                            content += $"{item.LowerPropertyName}: {Regex.Match(item.DefaultValue, @"\d+").Value},";
                            break;
                        case "Switch":
                            content += $"{item.LowerPropertyName}: {(item.DefaultValue == "1" ? true.ToString().ToLower() : false.ToString().ToLower())},";
                            break;
                        case "DatePicker"://忽略适配日期格式  
                            break;
                        default:
                            content += $"{item.LowerPropertyName}: \"{item.DefaultValue}\",";// 如果是字符串 DefaultValue=('男')
                            break;
                    }
                }
            }
        }
        return content;
    }
}