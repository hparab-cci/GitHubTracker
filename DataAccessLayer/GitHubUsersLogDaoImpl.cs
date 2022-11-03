using GitHubTracker.NHibernateMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.DataAccessLayer
{
    public class GitHubUsersLogDaoImpl : DaoProviderBase<GitHubUsersLogImpl, int>
    {
        public GitHubUsersLog CreateLog(GitHubUsersLog log)
        {
            return Create<GitHubUsersLog>(log);
        }

        public List<GitHubUsersLog> GetLogs()
        {
            return GetAll<GitHubUsersLog>();
        }

        public GitHubUsersLog GetLogById(int id)
        {
            return GetItemById<GitHubUsersLog, int>(id);
        }
    }    
}