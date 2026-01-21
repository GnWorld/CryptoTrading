

using Aop.Api.Domain;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Admin.NET.Core.Service;

public class AlipayFundTransUniTransferInput
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 商户AppId
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 商家订单号
    /// </summary>
    public string OutBizNo { get; set; }

    /// <summary>
    /// 转账金额
    /// </summary>
    public decimal TransAmount { get; set; }

    /// <summary>
    /// 业务标题
    /// </summary>
    public string OrderTitle { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 是否展示付款方别名
    /// </summary>
    public bool PayerShowNameUseAlias { get; set; }

    /// <summary>
    /// 收款方证件类型
    /// </summary>
    public AlipayCertTypeEnum? CertType { get; set; }

    /// <summary>
    /// 收款方证件号码，条件必填
    /// </summary>
    public string CertNo { get; set; }

    /// <summary>
    /// 收款方身份标识
    /// </summary>
    public string Identity { get; set; }

    /// <summary>
    /// 收款方真实姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 收款方身份标识类型
    /// </summary>
    public AlipayIdentityTypeEnum? IdentityType { get; set; }
}

/// <summary>
///  统一收单下单并支付页面接口输入参数
/// </summary>
public class AlipayTradePagePayInput
{
    /// <summary>
    /// 商户订单号
    /// </summary>
    [Required(ErrorMessage = "商户订单号不能为空")]
    public string OutTradeNo { get; set; }

    /// <summary>
    /// 订单总金额
    /// </summary>
    [Required(ErrorMessage = "订单总金额不能为空")]
    public string TotalAmount { get; set; }

    /// <summary>
    /// 订单标题
    /// </summary>
    [Required(ErrorMessage = "订单标题不能为空")]
    public string Subject { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// 超时时间
    /// </summary>
    public string TimeoutExpress { get; set; }

    /// <summary>
    /// 二维码宽度
    /// </summary>
    [Required(ErrorMessage = "二维码宽度不能为空")]
    public int? QrcodeWidth { get; set; }

    /// <summary>
    /// 业务参数
    /// </summary>
    public ExtendParams ExtendParams { get; set; }

    /// <summary>
    /// 商户业务数据
    /// </summary>
    public Dictionary<string, object> BusinessParams { get; set; }

    /// <summary>
    /// 开票信息
    /// </summary>
    public InvoiceInfo InvoiceInfo { get; set; }

    /// <summary>
    /// 外部买家信息
    /// </summary>
    public ExtUserInfo ExtUserInfo { get; set; }
}

public class AlipayPreCreateInput
{
    /// <summary>
    /// 商户订单号
    /// </summary>
    [Required(ErrorMessage = "商户订单号不能为空")]
    public string OutTradeNo { get; set; }

    /// <summary>
    /// 订单总金额
    /// </summary>
    [Required(ErrorMessage = "订单总金额不能为空")]
    public string TotalAmount { get; set; }

    /// <summary>
    /// 订单标题
    /// </summary>
    [Required(ErrorMessage = "订单标题不能为空")]
    public string Subject { get; set; }

    /// <summary>
    /// 超时时间
    /// </summary>
    public string TimeoutExpress { get; set; }
}

public class AlipayAuthInfoInput
{
    /// <summary>
    /// 用户Id
    /// </summary>

    [JsonProperty("user_id")]
    [JsonPropertyName("user_id")]
    [FromQuery(Name = "user_id")]
    public string UserId { get; set; }

    /// <summary>
    /// 授权码
    /// </summary>
    [JsonProperty("auth_code")]
    [JsonPropertyName("auth_code")]
    [FromQuery(Name = "auth_code")]
    public string AuthCode { get; set; }
}