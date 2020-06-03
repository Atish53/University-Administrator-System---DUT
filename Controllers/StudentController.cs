using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DUTAdmin.Models;
using System.Net;
using System.Threading.Tasks;

namespace DUTAdmin.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
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
        public async Task<ActionResult> CreateStudentAsync([Bind(Include = "Id,StudentNo,FirstName,LastName,Email,HomeAddress,Mobile,StudentPhoto,IsActive")] Student item)
        {
            if (ModelState.IsValid)
            {
                await DBRepository<Student>.CreateStudentAsync(item);
                return RedirectToAction("StudentDetails");
            }
            return View(item);
        }

        [HttpPost]
        [ActionName("EditStudent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStudentAsync([Bind(Include = "Id,StudentNo,FirstName,LastName,Email,HomeAddress,Mobile,StudentPhoto,IsActive")] Student item)
        {
            if (ModelState.IsValid)
            {
                await DBRepository<Student>.UpdateStudentAsync(item.StudentNo, item);
                return RedirectToAction("StudentIndex");
            }
            return View(item);
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

        [ActionName("DeleteStudent")]
        public async Task<ActionResult> DeleteStudentAsync(string id, string studentNo)
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

        [HttpPost]
        [ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteStudentConfirmedAsync([Bind(Include = "Id, StudentNo")]string id, string studentNo)
        {
            await DBRepository<Student>.DeleteStudentAsync(id, studentNo);
            return RedirectToAction("StudentIndex");
        }

        [ActionName("StudentDetails")]
        public async Task<ActionResult> StudentDetailsAsync(string id, string studentNo)
        {
            await DBRepository<Student>.DeleteStudentAsync(id, studentNo);
            return RedirectToAction("StudentIndex");
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

        // GET: Student
        [ActionName("StudentIndex")]
        public async Task<ActionResult> IndexAsync()
        {
            var student = await DBRepository<Student>.GetStudentsAsync(d => !d.IsActive || d.IsActive);
            return View(student);
        }

        Context db = new Context();
    }
}