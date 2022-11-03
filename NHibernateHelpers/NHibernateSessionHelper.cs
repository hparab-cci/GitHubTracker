using GitHubTracker.NHibernateMapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.NHibernateHelpers
{
    public static class NHibernateSessionHelper
    {
        public static ISessionFactory GetNhibirenateSessionfactory()
        {
            var configuration = new Configuration();

            configuration.DataBaseIntegration(config => { 
            
                config.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
                config.Driver<NHibernate.Driver.SqlClientDriver>();
                config.ConnectionStringName = "GitHubTrackerAppConnectionString";
                config.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();
            });

            configuration.CurrentSessionContext<WebSessionContext>();
            configuration.AddAssembly("GitHubTracker");
            return configuration.BuildSessionFactory();
        }
    }
}