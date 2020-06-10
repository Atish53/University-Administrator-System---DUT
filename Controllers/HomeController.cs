using DUTAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DUTAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateStudent()
        {
            ViewBag.Message = "Create a new student.";
            return RedirectToAction("CreateStudent", "StudentController");
        }

        public ActionResult UploadBlob(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadBlob(HttpPostedFileBase uploadFile)
        {
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }
            // Container Name - picture  
            BlobStorageManager BlobManagerObj = new BlobStorageManager("studentphoto");
            string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);

            return RedirectToAction("Get");
        }

        public ActionResult Get()
        {
            // Container Name - picture  
            BlobStorageManager BlobManagerObj = new BlobStorageManager("studentphoto");
            List<string> fileList = BlobManagerObj.BlobList();
            return View(fileList);
        }

        public ActionResult Delete(string uri)
        {
            // Container Name - picture  
            BlobStorageManager BlobManagerObj = new BlobStorageManager("picture");
            BlobManagerObj.DeleteBlob(uri);
            return RedirectToAction("Get");
        }

       
    }
}