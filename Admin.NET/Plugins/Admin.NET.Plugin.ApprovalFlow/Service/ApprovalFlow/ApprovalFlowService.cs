

using System.Text.Json;

namespace Admin.NET.Plugin.ApprovalFlow.Service;

/// <summary>
/// 审批流程服务
/// </summary>
[ApiDescriptionSettings(ApprovalFlowConst.GroupName, Order = 100)]
public class ApprovalFlowService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<ApprovalFlow> _approvalFlowRep;

    public ApprovalFlowService(SqlSugarRepository<ApprovalFlow> approvalFlowRep)
    {
        _approvalFlowRep = approvalFlowRep;
    }

    /// <summary>
    /// 分页查询审批流
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<ApprovalFlowOutput>> Page(ApprovalFlowInput input)
    {
        return await _approvalFlowRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Keyword), u => u.Code.Contains(input.Keyword.Trim()) || u.Name.Contains(input.Keyword.Trim()) || u.Remark.Contains(input.Keyword.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
            .Select<ApprovalFlowOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加审批流
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    public async Task<long> Add(AddApprovalFlowInput input)
    {
        var entity = input.Adapt<ApprovalFlow>();
        if (input.Code == null)
        {
            entity.Code = await LastCode("");
        }
        await _approvalFlowRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 更新审批流
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    public async Task Update(UpdateApprovalFlowInput input)
    {
        var entity = input.Adapt<ApprovalFlow>();
        await _approvalFlowRep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除审批流
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    public async Task Delete(DeleteApprovalFlowInput input)
    {
        var entity = await _approvalFlowRep.GetByIdAsync(input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _approvalFlowRep.FakeDeleteAsync(entity);  // 假删除
    }

    /// <summary>
    /// 获取审批流
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ApprovalFlow> GetDetail([FromQuery] QueryByIdApprovalFlowInput input)
    {
        return await _approvalFlowRep.GetByIdAsync(input.Id);
    }

    /// <summary>
    /// 根据编码获取审批流信息
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<ApprovalFlow> GetInfo([FromQuery] string code)
    {
        return await _approvalFlowRep.GetFirstAsync(u => u.Code == code);
    }

    /// <summary>
    /// 获取审批流列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<ApprovalFlowOutput>> GetList([FromQuery] ApprovalFlowInput input)
    {
        return await _approvalFlowRep.AsQueryable().Select<ApprovalFlowOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取今天创建的最大编号
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    private async Task<string> LastCode(string prefix)
    {
        var today = DateTime.Now.Date;
        var count = await _approvalFlowRep.AsQueryable().Where(u => u.CreateTime >= today).CountAsync();
        return prefix + DateTime.Now.ToString("yyMMdd") + string.Format("{0:d2}", count + 1);
    }

    [HttpGet]
    [ApiDescriptionSettings(Name = "FlowList")]
    [DisplayName("获取审批流结构")]
    public async Task<dynamic> FlowList([FromQuery] string code)
    {
        var result = await _approvalFlowRep.AsQueryable().Where(u => u.Code == code).Select<ApprovalFlowOutput>().FirstAsync();
        var FlowJson = result.FlowJson != null ? JsonSerializer.Deserialize<ApprovalFlowItem>(result.FlowJson) : new ApprovalFlowItem();
        var FormJson = result.FormJson != null ? JsonSerializer.Deserialize<ApprovalFormItem>(result.FormJson) : new ApprovalFormItem();
        return new
        {
            FlowJson,
            FormJson
        };
    }

    [HttpGet]
    [ApiDescriptionSettings(Name = "FormRoutes")]
    [DisplayName("获取审批流规则")]
    public async Task<List<string>> FormRoutes()
    {
        var results = await _approvalFlowRep.AsQueryable().Select<ApprovalFlowOutput>().ToListAsync();
        var list = new List<string>();
        foreach (var item in results)
        {
            var FormJson = item.FormJson != null ? JsonSerializer.Deserialize<ApprovalFormItem>(item.FormJson) : new ApprovalFormItem();
            if (item.FormJson != null) list.Add(FormJson.Route);
        }
        return list;
    }
}