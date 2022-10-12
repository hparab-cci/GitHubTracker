using GitHubTracker.NHibernateMapping;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.DataAccessLayer
{
    public class GitHubUsersDaoImpl : DaoProviderBase<GitHubUsersImpl, long>, IGitHubUsersDao
    {
        

        public GitHubUsers CreateUser(GitHubUsers user)
        {
            return Create<GitHubUsers>(user); 
        }

        public List<GitHubUsers> GetUsers()
        {
            return GetAll<GitHubUsers>();
        }
    }
}