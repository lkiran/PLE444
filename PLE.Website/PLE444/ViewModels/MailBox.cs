using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class MailBox
    {
        public List<PrivateMessage> Inbox { get; set; }
        public List<PrivateMessage> Sent { get; set; }
    }
}