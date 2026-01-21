

namespace Admin.NET.Core.Service;

public class SSHFileProvider : ICustomFileProvider, ITransient
{
    /// <summary>
    /// 创建SSH连接助手
    /// </summary>
    /// <returns></returns>
    private SSHHelper CreateSSHHelper()
    {
        return new SSHHelper(
            App.Configuration["SSHProvider:Host"],
            App.Configuration["SSHProvider:Port"].ToInt(),
            App.Configuration["SSHProvider:Username"],
            App.Configuration["SSHProvider:Password"]);
    }

    /// <summary>
    /// 构建文件完整路径
    /// </summary>
    /// <param name="sysFile"></param>
    /// <returns></returns>
    private string BuildFilePath(SysFile sysFile)
    {
        return string.Concat(sysFile.FilePath, "/", sysFile.Id + sysFile.Suffix);
    }

    public Task DeleteFileAsync(SysFile sysFile)
    {
        var fullPath = BuildFilePath(sysFile);
        using var helper = CreateSSHHelper();
        helper.DeleteFile(fullPath);
        return Task.CompletedTask;
    }

    public Task<string> DownloadFileBase64Async(SysFile sysFile)
    {
        using var helper = CreateSSHHelper();
        return Task.FromResult(Convert.ToBase64String(helper.ReadAllBytes(sysFile.FilePath)));
    }

    public Task<FileStreamResult> GetFileStreamResultAsync(SysFile sysFile, string fileName)
    {
        var filePath = BuildFilePath(sysFile);
        using var helper = CreateSSHHelper();
        return Task.FromResult(new FileStreamResult(helper.OpenRead(filePath), "application/octet-stream")
        {
            FileDownloadName = fileName + sysFile.Suffix
        });
    }

    public Task<SysFile> UploadFileAsync(IFormFile file, SysFile sysFile, string path, string finalName)
    {
        var fullPath = string.Concat(path.StartsWith('/') ? path : "/" + path, "/", finalName);
        using var helper = CreateSSHHelper();
        helper.UploadFile(file.OpenReadStream(), fullPath);
        return Task.FromResult(sysFile);
    }
}