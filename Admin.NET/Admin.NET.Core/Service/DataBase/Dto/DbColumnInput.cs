

namespace Admin.NET.Core.Service;

public class DbColumnInput
{
    public string ConfigId { get; set; }

    public string TableName { get; set; }

    public string DbColumnName { get; set; }

    public string DataType { get; set; }

    public int Length { get; set; }

    public string ColumnDescription { get; set; }

    public int IsNullable { get; set; }

    public int IsIdentity { get; set; }

    public int IsPrimarykey { get; set; }

    public int DecimalDigits { get; set; }

    public string DefaultValue { get; set; }
}

public class UpdateDbColumnInput
{
    public string ConfigId { get; set; }

    public string TableName { get; set; }

    public string ColumnName { get; set; }

    public string OldColumnName { get; set; }

    public string Description { get; set; }

    public string DefaultValue { get; set; }
}

public class MoveDbColumnInput
{
    /// <summary>
    /// 数据库配置ID
    /// </summary>
    public string ConfigId { get; set; }

    /// <summary>
    /// 目标表名
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    ///要移动的列名
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 移动到该列后方（为空时移动到首列）
    /// </summary>
    public string AfterColumnName { get; set; }
}

public class DeleteDbColumnInput
{
    public string ConfigId { get; set; }

    public string TableName { get; set; }

    public string DbColumnName { get; set; }
}