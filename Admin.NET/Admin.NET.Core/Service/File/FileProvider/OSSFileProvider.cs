

using OnceMi.AspNetCore.OSS;

namespace Admin.NET.Core.Service;

public class OSSFileProvider : ICustomFileProvider, ITransient
{
    private readonly IOSSService _OSSService;
    private readonly OSSProviderOptions _OSSProviderOptions;

    public OSSFileProvider(IOptions<OSSProviderOptions> oSSProviderOptions, IOSSServiceFactory ossServiceFactory)
    {
        _OSSProviderOptions = oSSProviderOptions.Value;
        if (_OSSProviderOptions.Enabled)
            _OSSService = ossServiceFactory.Create(Enum.GetName(_OSSProviderOptions.Provider));
    }

    public async Task DeleteFileAsync(SysFile file)
    {
        await _OSSService.RemoveObjectAsync(file.BucketName, string.Concat(file.FilePath, "/", $"{file.Id}{file.Suffix}"));
    }

    public async Task<string> DownloadFileBase64Async(SysFile file)
    {
        using var httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(file.Url);
        if (response.IsSuccessStatusCode)
        {
            // 读取文件内容并将其转换为 Base64 字符串
            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
            return Convert.ToBase64String(fileBytes);
        }
        throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
    }

    public async Task<FileStreamResult> GetFileStreamResultAsync(SysFile file, string fileName)
    {
        var filePath = Path.Combine(file.FilePath ?? "", file.Id + file.Suffix);
        var httpRemoteService = App.GetRequiredService<IHttpRemoteService>();
        var stream = await httpRemoteService.GetAsStreamAsync(await _OSSService.PresignedGetObjectAsync(file.BucketName, filePath, 5));
        return new FileStreamResult(stream, "application/octet-stream") { FileDownloadName = fileName + file.Suffix };
    }

    public async Task<SysFile> UploadFileAsync(IFormFile file, SysFile sysFile, string path, string finalName)
    {
        sysFile.Provider = Enum.GetName(_OSSProviderOptions.Provider);
        var filePath = string.Concat(path, "/", finalName);
        await _OSSService.PutObjectAsync(sysFile.BucketName, filePath, file.OpenReadStream());
        //  http://<你的bucket名字>.oss.aliyuncs.com/<你的object名字>
        //  生成外链地址 方便前端预览
        switch (_OSSProviderOptions.Provider)
        {
            case OSSProvider.Aliyun:
                sysFile.Url = $"{(_OSSProviderOptions.IsEnableHttps ? "https" : "http")}://{sysFile.BucketName}.{_OSSProviderOptions.Endpoint}/{filePath}";
                break;

            case OSSProvider.QCloud:
                var protocol = _OSSProviderOptions.IsEnableHttps ? "https" : "http";
                sysFile.Url = !string.IsNullOrWhiteSpace(_OSSProviderOptions.CustomHost)
                    ? $"{protocol}://{_OSSProviderOptions.CustomHost}/{filePath}"
                    : $"{protocol}://{sysFile.BucketName}-{_OSSProviderOptions.Endpoint}.cos.{_OSSProviderOptions.Region}.myqcloud.com/{filePath}";
                break;

            case OSSProvider.Minio:
                // 获取Minio文件的下载或者预览地址
                // newFile.Url = await GetMinioPreviewFileUrl(newFile.BucketName, filePath);// 这种方法生成的Url是有7天有效期的，不能这样使用
                // 需要在MinIO中的Buckets开通对 Anonymous 的readonly权限
                var customHost = _OSSProviderOptions.CustomHost;
                if (string.IsNullOrWhiteSpace(customHost))
                    customHost = _OSSProviderOptions.Endpoint;
                sysFile.Url = $"{(_OSSProviderOptions.IsEnableHttps ? "https" : "http")}://{customHost}/{sysFile.BucketName}/{filePath}";
                break;
        }
        return sysFile;
    }
}