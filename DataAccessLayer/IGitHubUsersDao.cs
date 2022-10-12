using GitHubTracker.NHibernateMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubTracker.DataAccessLayer
{
    public interface IGitHubUsersDao
    {
        List<GitHubUsers> GetUsers();
    }
}
