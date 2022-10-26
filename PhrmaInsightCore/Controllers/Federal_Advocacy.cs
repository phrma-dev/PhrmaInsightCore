using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PhrmaInsightCore.Models;
using PhrmaInsightCore.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhrmaInsightCore.Controllers
{
    [Authorize]
    public class Federal_AdvocacyController : Controller
    {
        private readonly ILogger<Federal_AdvocacyController> _logger;
        DatabaseContext _context;

        public Federal_AdvocacyController(ILogger<Federal_AdvocacyController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Tool()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetNote(string memberid)
        {
            try
            {
                var note = _context.MemberNotes.Where(a => a.MemberId == memberid).Select(a => a.Note).FirstOrDefault().ToString();
                return Json(note);
            }
            catch (Exception)
            {
                return Json("Add note here...");
            }

        }

        [HttpPost]
        public async Task<JsonResult> AddFolder([FromForm] string foldername)
        {

            string message = "";
            List<string> Folders = new List<string>();
            Folders = await _context.FederalAdvocacyFolders.Select(a => a.FolderName).ToListAsync<string>();
            bool exists = Folders.Contains(foldername);

            FederalAdvocacyFolders NewFolder = new FederalAdvocacyFolders();
            NewFolder.FolderName = foldername;
            if (!exists)
            {
                await _context.FederalAdvocacyFolders.AddAsync(NewFolder);
                await _context.SaveChangesAsync();
                message = "Success";
            } else
            {
                message = "Failure";
            }

            return Json(message);
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteFolder(string foldername) {

            var message = "";
            FederalAdvocacyFolders delete_folder = await _context.FederalAdvocacyFolders.Where(a => a.FolderName == foldername).FirstOrDefaultAsync();
            bool hasFiles = _context.FederalAdvocacyFiles.Where(a => a.Folder == foldername).ToList().Count() > 0;
            if (!hasFiles)
            {
                message = "Success";
                _context.FederalAdvocacyFolders.Remove(delete_folder);
                await _context.SaveChangesAsync();
            }
            else {
                message = "Failure";
            }

            return Json(message);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateMetadata([FromForm] string documentcategory, string filename, string members, string state, string meetingdate)
        {   //string documentcategory, string year, string members

            FederalAdvocacyFiles newFile = _context.FederalAdvocacyFiles.Where(a => a.FileName == filename).FirstOrDefault();
            newFile.DocumentCategory = documentcategory == null ? "" : documentcategory;
            newFile.MeetingDate = meetingdate == null ? "" : meetingdate;
            newFile.Members = members.Replace("\"", "").Replace("\"", "");
            newFile.Year = "";
            newFile.State = state == null ? "" : state;
            _context.FederalAdvocacyFiles.Update(newFile);
            await _context.SaveChangesAsync();
            return Json("OK");
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteFile(string filename)
        {
            string connectionString = "SharedAccessSignature=sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&st=2020-04-02T13%3A29%3A35Z&se=2099-12-31T05%3A00%3A00Z&sig=5Q6EY1TCODrEorNj0%2F1CO%2FVpOVH%2BCwpXu74z0NQyN7Y%3D;BlobEndpoint=https://phrmaanalytics.blob.core.windows.net/;FileEndpoint=https://phrmaanalytics.file.core.windows.net/;QueueEndpoint=https://phrmaanalytics.queue.core.windows.net/;TableEndpoint=https://phrmaanalytics.table.core.windows.net/;";

            FederalAdvocacyFiles delete_file = _context.FederalAdvocacyFiles.Where(a => a.FileName == filename).FirstOrDefault();
            _context.FederalAdvocacyFiles.Remove(delete_file);
            _context.SaveChanges();

            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("federal-advocacy");
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename);
            await cloudBlockBlob.DeleteAsync();
            // << Uploading the file to the blob >>  
            return Json("Success");
        }

        [HttpPost]
        public async Task<JsonResult> UploadFile([FromForm] IFormFile ifile, string folder)
        {   //string documentcategory, string year, string members

            FederalAdvocacyFiles newFile = new FederalAdvocacyFiles();
            newFile.Folder = folder;
            newFile.DocumentCategory = "";
            newFile.Year = "";
            newFile.FileName = ifile.FileName;
            newFile.MeetingDate = "";
            newFile.Members = "";
            newFile.State = "";
            _context.FederalAdvocacyFiles.Add(newFile);
            await _context.SaveChangesAsync();

            //Copy the storage account connection string from Azure portal     
            string connectionString = "SharedAccessSignature=sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&st=2020-04-02T13%3A29%3A35Z&se=2099-12-31T05%3A00%3A00Z&sig=5Q6EY1TCODrEorNj0%2F1CO%2FVpOVH%2BCwpXu74z0NQyN7Y%3D;BlobEndpoint=https://phrmaanalytics.blob.core.windows.net/;FileEndpoint=https://phrmaanalytics.file.core.windows.net/;QueueEndpoint=https://phrmaanalytics.queue.core.windows.net/;TableEndpoint=https://phrmaanalytics.table.core.windows.net/;";

            StreamReader streamReader = new StreamReader(ifile.OpenReadStream());
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("federal-advocacy");
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(ifile.FileName);
            await cloudBlockBlob.UploadFromStreamAsync(streamReader.BaseStream);
            // << Uploading the file to the blob >>  


            return Json("OK");
        }


        public async Task<JsonResult> GetFolders()
        {

            //Uri uri = new Uri("https://phrmaanalytics.blob.core.windows.net/federal-advocacy?st=2020-04-02T12%3A09%3A33Z&se=2099-12-31T05%3A00%3A00Z&sp=racwdl&sv=2018-03-28&sr=c&sig=dJIYKbNEBMqbjXYm48c6sXdvubCo18kObfzfjjNd%2B3o%3D"); // as calculated above
            //var cloudBlobContainer = new CloudBlobContainer(uri);

            //var blobList = await cloudBlobContainer.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.All, int.MaxValue, null, null, null);
            //var items = (from blob in blobList
            //                     .Results
            //                     .ToList()
            //             select blob).ToList();
            //List<string> files = new List<string>();

            //JArray json = JArray.FromObject(items);
            //for (var i = 0; i < json.Count; i++)
            //{

            //    string name = json[i]["Name"].ToString();
            //    files.Add(name);
            //}
            var folders = await _context.FederalAdvocacyFolders.Select(a => a.FolderName).Distinct().ToListAsync();
            return Json(folders);
        }

        public async Task<JsonResult> GetFiles(string folder)
        {

            //Uri uri = new Uri("https://phrmaanalytics.blob.core.windows.net/federal-advocacy?st=2020-04-02T12%3A09%3A33Z&se=2099-12-31T05%3A00%3A00Z&sp=racwdl&sv=2018-03-28&sr=c&sig=dJIYKbNEBMqbjXYm48c6sXdvubCo18kObfzfjjNd%2B3o%3D"); // as calculated above
            //var cloudBlobContainer = new CloudBlobContainer(uri);

            //var blobList = await cloudBlobContainer.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.All, int.MaxValue, null, null, null);
            //var items = (from blob in blobList
            //                     .Results
            //                     .ToList()
            //             select blob).ToList();
            //List<string> files = new List<string>();

            //JArray json = JArray.FromObject(items);
            //for (var i = 0; i < json.Count; i++)
            //{

            //    string name = json[i]["Name"].ToString();
            //    files.Add(name);
            //}
            var files = await _context.FederalAdvocacyFiles.Where(a => a.Folder == folder).ToListAsync();
            return Json(files);
        }


        public async Task<FileResult> GetBlobItems(string filename)
        {

            Uri uri = new Uri("https://phrmaanalytics.blob.core.windows.net/federal-advocacy?st=2020-04-02T12%3A09%3A33Z&se=2099-12-31T05%3A00%3A00Z&sp=racwdl&sv=2018-03-28&sr=c&sig=dJIYKbNEBMqbjXYm48c6sXdvubCo18kObfzfjjNd%2B3o%3D"); // as calculated above
            var cloudBlobContainer = new CloudBlobContainer(uri);

            //var blobList = await cloudBlobContainer.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.All, int.MaxValue, null, null, null);
            //var items = (from blob in blobList
            //                     .Results
            //                     .ToList()
            //             select blob).ToList();
            var items = cloudBlobContainer.ListBlobs();

            List<string> files = new List<string>();
            foreach (var item in items)
            {
                string name = item.Uri.ToString().Split("https://phrmaanalytics.blob.core.windows.net/federal-advocacy/")[1].ToString().Replace("%20", "");
                files.Add(name);
            }

            string storageAccount_connectionString = "SharedAccessSignature=sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&st=2020-04-02T13%3A29%3A35Z&se=2099-12-31T05%3A00%3A00Z&sig=5Q6EY1TCODrEorNj0%2F1CO%2FVpOVH%2BCwpXu74z0NQyN7Y%3D;BlobEndpoint=https://phrmaanalytics.blob.core.windows.net/;FileEndpoint=https://phrmaanalytics.file.core.windows.net/;QueueEndpoint=https://phrmaanalytics.queue.core.windows.net/;TableEndpoint=https://phrmaanalytics.table.core.windows.net/;";

            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(storageAccount_connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("federal-advocacy");

            CloudBlockBlob blob = container.GetBlockBlobReference(filename);



            Stream blobStream = blob.OpenRead();
            return File(blobStream, blob.Properties.ContentType, filename);


            // return Json(files);
        }

        [HttpPost]
        public JsonResult MemberNotes([FromBody] MemberNote note)
        {
            MemberNote new_note = new MemberNote();
            new_note.MemberId = note.MemberId;
            new_note.User = User.Identity.Name.ToLower();
            new_note.Note = note.Note;
            if (!_context.MemberNotes.Select(a => a.MemberId).Contains(new_note.MemberId))
            {
                _context.MemberNotes.Add(new_note);
                _context.SaveChanges();
            }
            else
            {
                MemberNote updateNote = _context.MemberNotes.Where(a => a.MemberId == note.MemberId).FirstOrDefault();
                updateNote.Note = note.Note;
                _context.MemberNotes.Update(updateNote);
                _context.SaveChanges();
            }

            return Json(note.Note.ToString());
        }

        [HttpGet]
        public async Task<JsonResult> GetMembersWithItems() 
        {
            var memberIdsWithItems = _context.FederalAdvocacyFiles.Select(a => a.Members).Distinct().ToList();
            List<string> memberIds = new List<string>();
            foreach (var item in memberIdsWithItems)
            {
                var ids = item.Split("|").ToList();
                foreach (var id in ids)
                {
                    var isCaptured = memberIds.Contains(id);
                    if (!isCaptured && id != "" && id != " ")
                    {
                        memberIds.Add(id);
                    }
                     
                }
            }

            List<Member> members = new List<Member>();

            var senators = await _context.Senators.ToListAsync();
            var house_reps = await _context.HouseMembers.ToListAsync();
            foreach (var senator in senators.OrderBy(a => a.LastName))
            {
                Member member = new Member()
                {
                    firstName = senator.FirstName,
                    lastName = senator.LastName,
                    proRepublicaId = senator.ProRepublicaId,
                    party = senator.Party,
                    image = senator.Image
                };
                members.Add(member);
            }

            foreach (var rep in house_reps.OrderBy(a => a.LastName))
            {
                Member member = new Member()
                {
                    firstName = rep.FirstName,
                    lastName = rep.LastName,
                    proRepublicaId = rep.ProPublicaId,
                    party = rep.Party,
                    image = rep.Image
                };
                members.Add(member);
            }

            var returnJSON = members.Where(a => memberIds.Contains(a.proRepublicaId)).ToList();
            return Json(returnJSON);
        }

        [HttpGet]
        public JsonResult GetSharepointDocuments()
        {
            List<SharePointDocument> ListShp = new List<SharePointDocument>();

            foreach (var item in _context.FederalAdvocacyFiles.ToList())
            {
                SharePointDocument shp = new SharePointDocument();
                List<string> members = new List<string>();
                bool isList = item.Members.ToString().Contains("|");
                members = item.Members.Split("|").ToList();
                List<string> member_names = new List<string>();

                foreach (var memberid in members)
                {
                    try
                    {
                        var name = _context.Senators.Where(a => a.ProRepublicaId == memberid).Select(a => a.FirstName + " " + a.LastName).FirstOrDefault();
                        member_names.Add(name.ToString());
                    }
                    catch (Exception)
                    {


                    }

                }
                shp.Document_Name = item.FileName;
                shp.Document_Type = item.DocumentCategory;
                shp.Members = member_names;
                shp.State = item.State;
                shp.Web_Url = "#";
                shp.Id = item.Id;
                ListShp.Add(shp);

            }
            return Json(ListShp);
        }

        public class Member
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string proRepublicaId { get; set; }
            public string party { get; set; }
            public string image { get; set; }
        }

        [HttpGet]
        public async Task<JsonResult> MembersGET()
        {
            List<Member> members = new List<Member>();

            var senators = await _context.Senators.ToListAsync();
            var house_reps = await _context.HouseMembers.ToListAsync();
            foreach (var senator in senators.OrderBy(a => a.LastName))
            {
                Member member = new Member()
                {
                    firstName = senator.FirstName,
                    lastName = senator.LastName,
                    proRepublicaId = senator.ProRepublicaId,
                    party = senator.Party,
                    image = senator.Image
                };
                members.Add(member);
            }

            foreach (var rep in house_reps.OrderBy(a => a.LastName))
            {
                Member member = new Member()
                {
                    firstName = rep.FirstName,
                    lastName = rep.LastName,
                    proRepublicaId = rep.ProPublicaId,
                    party = rep.Party,
                    image = rep.Image
                };
                members.Add(member);
            }
            return Json(members);

        }


    }
}
