using GitHubTracker.NHibernateMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.ViewModels
{
    public class VmUserDetails
    {
        public List<GitHubUsers> Users { get; set; }
    }
}