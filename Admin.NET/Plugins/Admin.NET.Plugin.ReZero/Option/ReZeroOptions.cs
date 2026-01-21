

namespace Admin.NET.Plugin.ReZero;

public sealed class ReZeroOptions : IConfigurableOptions
{
    /// <summary>
    /// AccessTokenKey
    /// </summary>
    public string AccessTokenKey { get; set; }

    /// <summary>
    /// RefreshAccessTokenKey
    /// </summary>
    public string RefreshAccessTokenKey { get; set; }
}