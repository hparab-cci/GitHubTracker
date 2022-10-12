using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.ViewModels
{
    public class VmDashboard
    {
        public int UserCount { get; set; }
        public int PublicRepoCount { get; set; }
        public int PublicGistCount { get; set; }
    }
}