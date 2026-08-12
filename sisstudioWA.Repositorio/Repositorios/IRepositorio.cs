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
        //Insert
        public Task<bool> Insert(E entity);
    }
}
