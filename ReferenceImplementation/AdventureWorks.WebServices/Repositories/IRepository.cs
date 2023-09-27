

using System.Collections.Generic;
using AdventureWorks.WebServices.Models;
using System.Linq;
using System;

namespace AdventureWorks.WebServices.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetItem(int id);
        T Create(T item);
        bool Update(T item);
        bool Delete(int id);
        void Reset();
    }
}