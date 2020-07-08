using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DUTAdmin.Models;
using DUTAdmin.ViewModels;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage.Blob;
using System.Web.UI.WebControls;

namespace DUTAdmin.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        Context db = new Context();

        // GET: Student
        [ActionName("StudentIndex")]
        public async Task<ActionResult> IndexAsync()
        {
            var student = await DBRepository<Student>.GetStudentsAsync(d => !d.IsActive || d.IsActive);
            return View(student);
        }

        [HttpPost]
        public async Task<ActionResult> Search(string name)
        {
            if ((ModelState.IsValid) && (!string.IsNullOrEmpty(name)))
            {

                var student = await DBRepository<Student>.GetStudentsAsync((a => (
                (a.FirstName == name) || (a.LastName == name) || (a.StudentNo == name) && (a.IsActive == true))));

                return View("StudentIndex", student);
            }
            return RedirectToAction("StudentIndex");

        }


#pragma warning disable 1998
        [ActionName("CreateStudent")]
        public async Task<ActionResult> CreateStudentAsync()
        {
            return View();
        }
#pragma warning disable 1998

        [HttpPost]
        [ActionName("CreateStudent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStudentAsync([Bind(Include = "Id,StudentNo,FirstName,LastName,Email,HomeAddress,Mobile,StudentPhoto,IsActive")] Student student, ViewModel photo)
        {
            if (ModelState.IsValid)
            {
                    var blobStorageManager = new BlobStorageManager();
                    await blobStorageManager.UploadPhotoAsync("studentphoto", photo.FileUpload);
                    string photoPath = blobStorageManager.GetFileURL("studentphoto", photo.FileUpload);
                    student.StudentPhoto = photoPath;
                    
                await DBRepository<Student>.CreateStudentAsync(student);                
            }
            return RedirectToAction("StudentIndex");
        }

        [HttpPost]
        [ActionName("EditStudent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStudentAsync([Bind(Include = "Id,StudentNo,FirstName,LastName,Email,HomeAddress,Mobile,StudentPhoto,IsActive")] ViewModel student)
        {
            
            if (ModelState.IsValid) 
            {
                if (student.FileUpload != null)
                {
                    var blobStorageManager = new BlobStorageManager();
                    await blobStorageManager.UploadPhotoAsync("studentphoto", student.FileUpload);
                    string photoPath = blobStorageManager.GetFileURL("studentphoto", student.FileUpload);                 
                    student.StudentPhoto = photoPath;
                    await DBRepository<ViewModel>.UpdateStudentAsync(student.Id, student);
                    return RedirectToAction("StudentIndex");
                }
                else
                {
                    await DBRepository<ViewModel>.UpdateStudentAsync(student.Id, student);
                    return RedirectToAction("StudentIndex");
                }                
            }
            return View(student);
        }

        [ActionName("EditStudent")]
        public async Task<ActionResult> EditStudentAsync(string id, string studentNo)
        {
            ViewModel viewModel = await DBRepository<ViewModel>.GetStudentAsync(id, studentNo);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                        
            if (viewModel == null)
            {
                return HttpNotFound();
            }

            return View(viewModel);
        }

        [ActionName("DeleteStudent")]
        public async Task<ActionResult> DeleteStudentAsync(string id, string studentNo)
        {
            if (id == null)
            {
               
                return RedirectToAction("Get");
            } 
            
               

            Student student = await DBRepository<Student>.GetStudentAsync(id, studentNo);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteStudentConfirmedAsync([Bind(Include = "Id, StudentNo, StudentPhoto")]string id, string studentNo, string studentPhoto)
        {
            var blobStorageManager = new BlobStorageManager();
            await blobStorageManager.DeleteBlob(studentPhoto);
            await DBRepository<Student>.DeleteStudentAsync(id, studentNo);
            return RedirectToAction("StudentIndex");
        }

        [ActionName("StudentDetails")]
        public async Task<ActionResult> StudentDetailsAsync(string id, string studentNo)
        {
            Student student = await DBRepository<Student>.GetStudentAsync(id, studentNo);
            return View(student);

        }
        

       
    }
}