

namespace Admin.NET.Plugin.WorkWeixin.Proxy;

public class AuthAccessTokenHttpOutput : BaseWorkOutput
{
    /// <summary>
    /// 获取到的凭证
    /// </summary>
    [JsonProperty("access_token")]
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// 凭证的有效时间（秒）
    /// </summary>
    [JsonProperty("expires_in")]
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}