using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DUTAdmin.Models;
using DUTAdmin.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage.Blob;
using System.Web.UI.WebControls;
using Syncfusion.XlsIO;
using System.ComponentModel.Composition.Primitives;
using System.Web.Services.Description;
using Microsoft.Azure.Documents;

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
        public async Task<ActionResult> CreateStudentAsync([Bind(Include = "Id,StudentNo,FirstName,LastName,Email,HomeAddress,Mobile,StudentPhoto,IsActive", Exclude = "To")] Student student, ViewModel photo)
        {
            if (ModelState.IsValid)
            {
                
                var blobStorageManager = new BlobStorageManager();
                blobStorageManager.UploadPhotoOptimistic("studentphoto", photo.FileUpload);
                string photoPath = blobStorageManager.GetFileURL("studentphoto", photo.FileUpload);
                student.StudentPhoto = photoPath;

                await DBRepository<Student>.CreateStudentAsync(student);
            }
            return RedirectToAction("StudentIndex");
        }

        [HttpPost]
        [ActionName("EditStudent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStudentAsync([Bind(Include = "Id,StudentNo,FirstName,LastName,Email,HomeAddress,Mobile,StudentPhoto,IsActive")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.StudentPhoto = student.StudentPhoto;
                await DBRepository<Student>.UpdateStudentAsync(student.Id, student);
                return RedirectToAction("StudentIndex");
            }
            return View(student);
        }

        [ActionName("EditStudent")]
        public async Task<ActionResult> EditStudentAsync(string id, string studentNo)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await DBRepository<Student>.GetStudentAsync(id, studentNo);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [ActionName("UpdatePhoto")]
        public async Task<ActionResult> UpdatePhotoAsync(string id, string studentNo, string studentPhoto)
        {
            Student student = await DBRepository<Student>.GetStudentAsync(id, studentNo);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (student == null)
            {
                return HttpNotFound();
            }

            if (studentPhoto == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ActionName("UpdatePhoto")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePhotoAsync([Bind(Include = "Id,StudentNo,FirstName,LastName,Email,HomeAddress,Mobile,IsActive,StudentPhoto")]Student student, HttpPostedFileBase fileUpload)
        {
           
            if (ModelState.IsValid)
            {
                var blobStorageManager = new BlobStorageManager();
                await blobStorageManager.DeleteBlob(student.StudentPhoto);
                blobStorageManager.UploadPhotoOptimistic("studentphoto", fileUpload);
                student.StudentPhoto = blobStorageManager.GetFileURL("studentphoto", fileUpload);
                


                await DBRepository<Student>.UpdateStudentAsync(student.Id, student);
            }
            return View(student);
        }

        [ActionName("DeleteStudent")]
        public async Task<ActionResult> DeleteStudentAsync(string id, string studentNo)
        {
            Student student = await DBRepository<Student>.GetStudentAsync(id, studentNo);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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



        [ActionName("ExportDetails")]
        public async Task<ActionResult> ExportDetails(string id, string studentNo)
        {
            Student student = await DBRepository<Student>.GetStudentAsync(id, studentNo);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ActionName("ExportDetails")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExportDetails(Student student)
        {
            await DBRepository<Student>.GetStudentAsync(student.Id, student.StudentNo);
            if (ModelState.IsValid)
            {
                
                
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    //Instantiate the Excel application object
                    IApplication application = excelEngine.Excel;

                    //Assigns default application version
                    application.DefaultVersion = ExcelVersion.Excel2013;

                    //A new workbook is created equivalent to creating a new workbook in Excel
                    //Create a workbook with 1 worksheet
                    IWorkbook workbook = application.Workbooks.Create(1);

                    //Access first worksheet from the workbook
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //Adding Headers
                    worksheet.Range["A1"].Text = "Student Number";
                    worksheet.Range["B1"].Text = "First Name";
                    worksheet.Range["C1"].Text = "Last Name";
                    worksheet.Range["D1"].Text = "Email Address";
                    worksheet.Range["E1"].Text = "Home Address";
                    worksheet.Range["F1"].Text = "Mobile";
                    worksheet.Range["G1"].Text = "Photo URI";
                    worksheet.Range["H1"].Text = "Active Status";

                    //Adding Student Information
                    worksheet.Range["A2"].Text = student.StudentNo;
                    worksheet.Range["B2"].Text = student.FirstName;
                    worksheet.Range["C2"].Text = student.LastName;
                    worksheet.Range["D2"].Text = student.Email;
                    worksheet.Range["E2"].Text = student.HomeAddress;
                    worksheet.Range["F2"].Text = student.Mobile;
                    worksheet.Range["G2"].Text = student.StudentPhoto;
                    worksheet.Range["H2"].Boolean = student.IsActive;

                    //Format Columns
                    worksheet.UsedRange.AutofitColumns();

                    //Save the workbook as stream
                    MemoryStream outputStream = new MemoryStream();
                    outputStream.Position = 0;
                    workbook.SaveAs(outputStream);

                    MailMessage mail = new MailMessage();
                    

                    string emailTo = student.To;
                    MailAddress from = new MailAddress("starkdev.email@gmail.com");
                    mail.From = from;
                    mail.Subject = "Student Information For" + " " + student.StudentNo;
                    mail.Body = "Dear " + emailTo + ", find the attached excel document containing the student information for " + student.FirstName + " " + student.LastName + ".";
                    mail.To.Add(emailTo);

                    outputStream.Position = 0;
                    var contentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Octet);
                    var studentDetailsAttachment = new System.Net.Mail.Attachment(outputStream, contentType);
                    studentDetailsAttachment.ContentDisposition.FileName = student.StudentNo + ".xlsx";

                    mail.Attachments.Add(studentDetailsAttachment);
                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential("starkdev.email@gmail.com", "StarkDev19");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);

                    mail.Dispose();
                }
                
            }
            return RedirectToAction("StudentIndex");
        }



    }
}