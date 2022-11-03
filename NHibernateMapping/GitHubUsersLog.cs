using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.NHibernateMapping
{
    public class GitHubUsersLog
    {
        public virtual int GitHubUsersLogId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int PublicRepoCount { get; set; }
        public virtual int PublicGistCount { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual int CreateUserId { get; set; }
        public static class Factory
        {
            public static GitHubUsersLog NewInstance()
            {
                return new GitHubUsersLogImpl();
            }
        }
    }
    public partial class GitHubUsersLogImpl : GitHubUsersLog
    {

    }
}