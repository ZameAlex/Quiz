using System;
using System.Collections.Generic;

namespace DataClasses
{
     public interface IRepository<T> : IDisposable
    {
        void SaveItems(ICollection<T> items, int ID);
        int SaveItem(T item, int ID);

         T GetItem(int ID, int ItemID);

         ICollection<T> GetItems(int ID);

         void DeleteItem(T item, int ID);

         void UpdateItem(T newItem, int ID);
    }
}
