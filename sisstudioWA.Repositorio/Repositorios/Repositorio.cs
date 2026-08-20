using sisstudioWA.BD.Datos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace sisstudioWA.Repositorio.Repositorios
{
    public class Repositorio<E> : IRepositorio<E> where E : class, IEntityBase
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
        public async Task<E> SelectById(int id)
        {
            var entidad = await _context.Set<E>().FirstOrDefaultAsync(x => x.Id  == id );
            if (entidad == null)
            {
                throw new Exception("No se encontró el registro");
            }
            else 
            {
                return entidad;
            }

        }
        //public async Task<bool> Exists(int id)
        //{
        //    var entidad = await _context.Set<E>().FirstOrDefaultAsync(x => x.Id == id);
        //    if (entidad == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
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
        public async Task<bool> Update(int id, E entidadNueva)
        {
            var entidad = await _context.Set<E>().FirstOrDefaultAsync<E>(x => x.Id == id);
            if (entidad == null)
            {
                return false;
            }
            else
            {
                foreach (var property in typeof(E).GetProperties()) //typeof me entrega la clase y GetProperties me entrega todas las propiedades de la clase
                {
                    if (property.Name == "Id") continue;
                    property.SetValue(entidad, property.GetValue(entidadNueva)); //property.Setvalue busca la propiedad en la entidad y le asigna el valor de la propiedad en la entidad nueva
                }

                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entidad = await _context.Set<E>().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return false;
            }
            else
            {
                _context.Set<E>().Remove(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
