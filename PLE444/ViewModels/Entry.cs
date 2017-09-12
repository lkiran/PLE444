using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class Entry
    {
        public List<Community> Community { get; set; }

        public List<Course> Course { get; set; }

        public List<Space> Space { get; set; }
    }
}