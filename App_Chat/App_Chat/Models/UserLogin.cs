using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Chat.Models
{
    public class UserLogin
    {
        private string Username { get; set; }
        private string Password { get; set; }
    }
}