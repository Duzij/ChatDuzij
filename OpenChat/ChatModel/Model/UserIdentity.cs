using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenChat.Models
{
    public class UserIdentity
    {
        [Key]
        public string Username { get; set; }

        public string ConnectionID { get; set; }
    }
}