

namespace Admin.NET.Core.Service;

public class CreateSeedDataInput
{
    /// <summary>
    /// 库标识
    /// </summary>
    public string ConfigId { get; set; }

    /// <summary>
    /// 表名
    /// </summary>
    /// <example>student</example>
    public string TableName { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    /// <example>Student</example>
    public string EntityName { get; set; }

    /// <summary>
    /// 种子名称
    /// </summary>
    /// <example>Student</example>
    public string SeedDataName { get; set; }

    /// <summary>
    /// 导出位置
    /// </summary>
    /// <example>Web.Application</example>
    public string Position { get; set; }

    /// <summary>
    /// 后缀
    /// </summary>
    /// <example>Web.Application</example>
    public string Suffix { get; set; }

    /// <summary>
    /// 过滤已有数据
    /// </summary>
    /// <remarks>
    /// 如果数据在其它不同名的已有的种子类型的数据中出现过，就不生成这个数据
    /// 主要用于生成菜单功能，菜单功能往往与子项目绑定，如果生成完整数据就会导致菜单项多处理重复。
    /// </remarks>
    public bool FilterExistingData { get; set; }
}