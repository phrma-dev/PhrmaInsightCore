using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PhrmaInsightCore.Models;
using PhrmaInsightCore.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhrmaInsightCore.Controllers
{

    [AllowAnonymous]
    public class ScreeningController : Controller
    {
        DatabaseContext _context;

        public ScreeningController(DatabaseContext context)
        {
            _context = context;

        }
        [AllowAnonymous]
        public IActionResult Screening() {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> UploadFile([FromForm] IFormFile ifile)
        {   //string documentcategory, string year, string members

            //Copy the storage account connection string from Azure portal     
            string connectionString = "SharedAccessSignature=sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&st=2020-04-02T13%3A29%3A35Z&se=2099-12-31T05%3A00%3A00Z&sig=5Q6EY1TCODrEorNj0%2F1CO%2FVpOVH%2BCwpXu74z0NQyN7Y%3D;BlobEndpoint=https://phrmaanalytics.blob.core.windows.net/;FileEndpoint=https://phrmaanalytics.file.core.windows.net/;QueueEndpoint=https://phrmaanalytics.queue.core.windows.net/;TableEndpoint=https://phrmaanalytics.table.core.windows.net/;";
            StreamReader streamReader = new StreamReader(ifile.OpenReadStream());
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("screening");
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(ifile.FileName);
            await cloudBlockBlob.UploadFromStreamAsync(streamReader.BaseStream);
            


            return Json("OK");
        }


    }
}