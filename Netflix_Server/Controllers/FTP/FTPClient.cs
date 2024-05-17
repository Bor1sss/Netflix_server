using System.Net;

namespace Netflix_Server.Controllers.FTP
{
    public class FTPClient
    {
        public async Task<Stream> DownloadFile(string ftpServerUrl, string ftpUsername, string ftpPassword, string filePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{ftpServerUrl}/{filePath}");
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                return response.GetResponseStream();
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public async Task UploadFile(string ftpServerUrl, string username, string password, string ftpPath, Stream fileStream)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{ftpServerUrl}/{ftpPath}");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);

            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                await fileStream.CopyToAsync(requestStream);
            }

            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }
        }
    }
}
