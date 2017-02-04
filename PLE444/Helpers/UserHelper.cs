using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace PLE444.Helpers
{
    public class UserHelper
    {
        private PleDbContext db = new PleDbContext();

        public string GetUserPhotoFromID(string id)
        {
            if (id != null)
            {
                if (db.Users.Find(id).ProfilePicture == null)
                    return "~/Content/img/pp.jpg";
                return db.Users.Find(id).ProfilePicture;
            }
            return "";
        }

        public string GetUserFullNameFromID(string id)
        {
            if (id != null)
                return db.Users.Find(id).FirstName + " " + db.Users.Find(id).LastName;
            return "";
        }
    }
}