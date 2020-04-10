using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    interface IRepository<T> where T : IEntity
    {
        public void Add(T entity);
        public IEnumerable<T> GetAll();
        public T GetById(int id);
        public void Update(T entity);
        void Delete(int id);
    }
}
