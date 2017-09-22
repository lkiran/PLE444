using Microsoft.AspNet.Identity;
using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class TimelineController : Controller
    {
        private PleDbContext db = new PleDbContext();

        // POST: Timeline/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid entryId, Guid courseId)
        {
            if(!isCourseCreator(courseId))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            try
            {
                var entry = db.TimelineEntries.Find(entryId);
                if (entry == null)
                    return HttpNotFound();

                db.TimelineEntries.Remove(entry);
                db.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        private bool isCourseCreator(Guid? courseId)
        {
            if (courseId == null)
                return false;

            var course = db.Courses.Find(courseId);
            return isCourseCreator(course);
        }

        private bool isCourseCreator(Course course)
        {
            if (course == null)
                return false;

            else if (course.CreatorId != User.GetPrincipal()?.User.Id)
                return false;
            return true;
        }
    }
}