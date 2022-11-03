using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronJob.DomainClasses
{
    public class GitHubUsersLog
    {
        public virtual int GitHubUsersLogId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int PublicRepoCount { get; set; }
        public virtual int PublicGistCount { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual int CreateUserId { get; set; }

    }
}
