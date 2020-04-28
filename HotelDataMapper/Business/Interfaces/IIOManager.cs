using System.Threading.Tasks;

namespace HotelDataMapper.Business.Interfaces
{
    public interface IIoManager
    {
        Task DownloadFile(string downloadUrl);
        Task ReadFile();
        void ExtractZipFile(string fileName, string folderName);
    }
}