﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UserStore.Data.Interfaces
{
    public interface IRepository<T,S> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(S id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
