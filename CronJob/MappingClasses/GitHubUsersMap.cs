using CronJob.DomainClasses;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronJob.MappingClasses
{
    public class GitHubUsersMap : ClassMap<GitHubUsers>
    {
        public GitHubUsersMap()
        {
            Id(c => c.UserId);
            Map(c => c.UserName);
            Map(c => c.Name);
            Map(c => c.PublicRepoCount);
            Map(c => c.PublicGistCount);
            Map(c => c.CreateDate);
            Map(c => c.CreateUserId);
            Map(c => c.UpdateDate);
        }
    }
}
