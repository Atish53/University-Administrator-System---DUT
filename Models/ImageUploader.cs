using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Azure.Storage.Blobs.Models;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System.IO;

namespace DUTAdmin.Models
{
    public class ImageUploader
    {
        public static async Task<bool> UploadImage(Stream fileStream, string fileName, AzureStorageConfig_storageConfig)
        {
            Uri blobUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/" + _storageConfig.ImageContainer + "/" + filename);

            //acc name - imagestre
            //key - NxlIbw10EPYwPll8lSGEAn6manL9mk5bharVJgjzZMDMFygKRCKu59kbgqshKOzZgeIIx8z6U6vLfdz+0fbkiw==
            //string - DefaultEndpointsProtocol=https;AccountName=imagestre;AccountKey=NxlIbw10EPYwPll8lSGEAn6manL9mk5bharVJgjzZMDMFygKRCKu59kbgqshKOzZgeIIx8z6U6vLfdz+0fbkiw==;EndpointSuffix=core.windows.net
        }
    }
}