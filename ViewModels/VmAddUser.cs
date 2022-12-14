using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GitHubTracker.ViewModels
{
    public class VmAddUser
    {
        [Required(ErrorMessage = "Required.")]
        public string UserName { get; set; }
        public string Name { get; set; }
        public int PublicRepoCount { get; set; }
        public int PublicGistCount { get; set; }
    }
}