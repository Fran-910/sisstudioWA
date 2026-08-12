using sisstudioWA.BD.Datos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace sisstudioWA.Repositorio.Repositorios
{
    public class Repositorio<E> : IRepositorio<E> where E : class
    {
        private readonly AppDbContext _context;
        public Repositorio(AppDbContext context) 
        {
            this._context = context;
        }
        public async Task<List<E>> Select()
        {
            var list = await _context.Set<E>().ToListAsync();
            if (list == null)
            {
                throw new Exception("No se encontraron registros");
            }
            return list;
        }
        public async Task<bool> Insert(E entity)
        {
            if (entity == null)
            {
                return false;
            }
            _context.Set<E>().Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
