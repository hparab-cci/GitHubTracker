using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.Models
{
    public class Users
    {       
        public string login { get; set; }
        public string name { get; set; }
        public int public_repos { get; set; } //or login ?
        public int public_gists { get; set; } 
    }
}