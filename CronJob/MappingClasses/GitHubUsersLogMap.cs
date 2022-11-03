using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CronJob.DomainClasses;
using FluentNHibernate.Mapping;

namespace CronJob.MappingClasses
{
    public class GitHubUsersLogMap : ClassMap<GitHubUsersLog>
    {
        public GitHubUsersLogMap()
        {
            Id(c => c.GitHubUsersLogId);
            Map(c => c.UserId);
            Map(c => c.PublicRepoCount);
            Map(c => c.PublicGistCount);
            Map(c => c.CreateDate);
            Map(c => c.CreateUserId);
        }
    }
}
