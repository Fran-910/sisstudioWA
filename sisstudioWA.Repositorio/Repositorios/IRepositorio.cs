using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Repositorio.Repositorios
{
    public interface IRepositorio<E> where E : class
    {
        //Select
        public Task<List<E>> Select();
        //Select by Id
        public Task<E> SelectById(int id);
        //Exists
        //public Task<bool> Exists(int id)
        //Insert
        public Task<bool> Insert(E entity);
        //Update
        public Task<bool> Update(int id,E entity);
        //Delete
        public Task<bool> Delete(int id);
    }
}
