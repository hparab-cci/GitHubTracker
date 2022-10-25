using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GitHubTracker.ViewModels
{
    public class VmUser
    {
        [Required(ErrorMessage = "Required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Password { get; set; }
    }
}