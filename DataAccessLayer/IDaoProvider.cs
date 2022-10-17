using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubTracker.DataAccessLayer
{
    public interface IDaoProvider<T, V> where T : new()
                                        where V : struct
    {
        T Create<T>(T t);
        List<T> GetAll<T>();
        T GetObjectById(T v);
        void Update(T t);
        void Delete(T t);
        T GetItemById<T, TId>(TId id);
    }
}
