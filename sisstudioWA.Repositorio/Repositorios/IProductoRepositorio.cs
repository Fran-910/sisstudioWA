using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Repositorio.Repositorios
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        Task<bool> InsertKit(Producto producto, List<int> idProductosComponentes);
        Task<List<ProductoPrivadoDTO>> GetProductosKitById(int id);
        Task<List<int>> GetIdProductosKit(int id);
        Task<bool> UpdateKit(int id, Producto producto, List<int> idProductosComponentes);
        Task<Producto?> GetProdImg(int id, string url);
        Task<bool> DeleteProd(int id);

    }
}
