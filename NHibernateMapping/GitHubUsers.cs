using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.NHibernateMapping
{
    public abstract class GitHubUsers
    {
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Name { get; set; }
        public virtual int PublicRepoCount { get; set; }
        public virtual int PublicGistCount { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual int CreateUserId { get; set; } 
        public virtual DateTime? UpdateDate { get; set; }
        public virtual string Remarks { get; set; }

        public static class Factory
        {
            public static GitHubUsers NewInstance()
            {
                return new GitHubUsersImpl();
            }
        }
    }
    public partial class GitHubUsersImpl : GitHubUsers
    {

    }
}