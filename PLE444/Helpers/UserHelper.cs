using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using PLE444.Context;
using System.Web.Mvc;

namespace PLE444.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext UserDB = new ApplicationDbContext();

        public string GetUserPhotoFromID(string id)
        {
            if (id != null)
                return UserDB.Users.Find(id).ProfilePicture;
            return "";
        }

        public string GetUserFullNameFromID(string id)
        {
            if (id != null)
                return UserDB.Users.Find(id).FirstName + " " + UserDB.Users.Find(id).LastName;
            return "";
        }
    }
}