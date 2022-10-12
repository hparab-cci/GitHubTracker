using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GitHubTracker.DataAccessLayer
{
    public class DaoProviderBase<T, V> : IDaoProvider<T, V> where T : new()
                                                           where V : struct
    {
        protected readonly ISession CurrentNhibernateSession;
        protected DaoProviderBase()
        {
            CurrentNhibernateSession = GitHubTracker.MvcApplication.NHibernateSessionFactory.GetCurrentSession();
        }
        public T Create<T>(T t)
        {
            using(ITransaction transaction = CurrentNhibernateSession.BeginTransaction())
            {
                this.CurrentNhibernateSession.Save(t);
                transaction.Commit();
            }
            return t;
        }

        public List<T> GetAll<T>()
        {
            var icriteria = this.CurrentNhibernateSession.CreateCriteria(typeof(T));
            return icriteria.List<T>().ToList();
        }

        public T GetObjectById(V v)
        {
            return (T) CurrentNhibernateSession.Load(typeof(T),v);
        }

        public void Update(T t)
        {
            using (ITransaction transaction = CurrentNhibernateSession.BeginTransaction())
            {
                this.CurrentNhibernateSession.SaveOrUpdate(t);
                transaction.Commit();
            }
        }

        public void Delete(T t)
        {
            using (ITransaction transaction = CurrentNhibernateSession.BeginTransaction())
            {
                this.CurrentNhibernateSession.Delete(t);
                transaction.Commit();
            }
        }
    }
}