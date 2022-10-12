using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.ViewModels
{
    public class VmAddUser
    {
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public int PublicRepoCount { get; set; }
        public int PublicGistCount { get; set; }
    }
}