using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HotelDataMapper.Business.Interfaces;
using HotelDataMapper.Data.Enums;
using Microsoft.EntityFrameworkCore.Internal;

namespace HotelDataMapper.Business.Classes
{
    public class IoManager : IIoManager
    {
        public async Task DownloadFile(string downloadUrl)
        {
            var splitUrl = downloadUrl.Split('/');
            // "https://cdndemo.partocrs.com/ApiDocument/StaticData/HotelStaticData.zip"
            // Ensure Folder Exists
            CreateFolderIfNotExists("MapperData", null);
            // Ensure File exists
            using WebClient client = new WebClient();
            client.DownloadFileCompleted += DownloadFileCallback;
            // Specify a progress notification handler.
            client.DownloadProgressChanged += DownloadProgressCallback;
            client.DownloadFileAsync(new Uri(downloadUrl),
                splitUrl?.LastOrDefault());

        }

        public async Task ReadFile()
        {
            throw new System.NotImplementedException();
        }

        private static void ExtractZipFile(string fileName, string folderName)
        {
            if (FindFile(fileName))
            {
                ZipFile.ExtractToDirectory($@"C:\MapperData\{fileName}.zip", $@"C:\MapperData\{folderName}",true);
            }
        }

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            Console.Write("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }

        private static void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine(e.UserState);
        }

        private void CreateFolderIfNotExists(string path, string folderName)
        {
            var dir = Directory.GetDirectories(@"c:\");
            if (dir.FirstOrDefault(a => String.Equals(a, "C:\\MapperData", StringComparison.CurrentCultureIgnoreCase)) == null)
            {
                Directory.CreateDirectory($@"C:\MapperData");
            }
            else if (!string.IsNullOrEmpty(folderName))
            {
                var subFolders = Directory.GetDirectories(@"c:\MapperData");
                if (subFolders.Length == 0)
                {
                    Directory.CreateDirectory($@"C:\{path}\{folderName}");
                }
                else if ((subFolders.FirstOrDefault(a =>
                    string.Equals(a.ToLower(), (folderName.ToLower()), StringComparison.Ordinal))) == null)
                {
                    Directory.CreateDirectory($@"C:\{path}\{folderName}");
                }
            }
        }

        private void DeleteOrClearFileOrFolder(string path, IoTypes type)
        {
            switch (type)
            {
                case IoTypes.SingleFile:
                    File.Delete($@"C:\{path}");
                    break;
                case IoTypes.Directory:
                    string[] files = Directory.GetFiles(path);
                    if (files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            File.Delete(file);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static bool FindFile(string fileName)
        {
            var files = Directory.GetFiles(@"C:\MapperData");
            var file = files.FirstOrDefault(a => a.Contains(fileName));

            return !string.IsNullOrEmpty(file);
        }
    }
}