using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronJob.DomainClasses
{
    public class GitHubUsers
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
    }
}
