using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    interface IRepository<T> where T : IEntity
    {
        public List<T> RepositoryContent { get; }
        public void Add(T entity);
        public void Update(T entity);
        void Delete(int id);
    }
}
