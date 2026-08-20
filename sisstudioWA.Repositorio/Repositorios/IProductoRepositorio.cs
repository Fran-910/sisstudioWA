using sisstudioWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Repositorio.Repositorios
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        Task<bool> InsertKit(Producto producto, List<int> idProductosComponentes);
        public Task<List<Producto>> GetProductosKitById(int id);
    }
}
