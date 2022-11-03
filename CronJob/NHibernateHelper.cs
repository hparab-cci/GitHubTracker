using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronJob
{
    public static class NHibernateHelper
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                            .ConnectionString(c => c.FromConnectionStringWithKey("connectionStringKey")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                                .ExposeConfiguration(cfg => new SchemaExport(cfg))
                .BuildSessionFactory();
        }
    }
}
