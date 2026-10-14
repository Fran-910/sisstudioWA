using Microsoft.EntityFrameworkCore;
using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace sisstudioWA.Repositorio.Repositorios
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly AppDbContext _context;

        public ProductoRepositorio(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<bool> InsertKit(Producto producto, List<int> idProductosComponentes)
        {
            // Insertar el producto en la tabla de productos
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            // Insertar el kit en la tabla de kitProductos

            foreach (var idProducto in idProductosComponentes)
            {

                var kitProducto = new KitProducto();
                kitProducto.KitId = producto.Id;
                kitProducto.ProductoId = idProducto;
                await _context.KitsProductos.AddAsync(kitProducto);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateKit(int id, Producto producto, List<int> idProductosComponentes)
        {
            var productoExistente = await _context.Productos.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (productoExistente == null)
            {
                Console.WriteLine("No se encontró el producto con ID: " + id);
                return false; // El producto no existe
            }
            else
            {
                productoExistente.Nombre = producto.Nombre;
                productoExistente.Subtitulo = producto.Subtitulo;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Imagenes = producto.Imagenes;
                productoExistente.tipoProd = producto.tipoProd;
                productoExistente.Precio = producto.Precio;
                productoExistente.Stock = producto.Stock;

                await _context.SaveChangesAsync();

                // Eliminar los componentes existentes del kit  
                var componentesExistentes = await _context.KitsProductos.Where(kp => kp.KitId == id).ToListAsync();

                foreach (var componente in componentesExistentes)
                {
                    _context.KitsProductos.Remove(componente);
                }

                foreach (var idProducto in idProductosComponentes)
                {

                    var kitProducto = new KitProducto();
                    kitProducto.KitId = productoExistente.Id;
                    kitProducto.ProductoId = idProducto;
                    await _context.KitsProductos.AddAsync(kitProducto);

                }

                await _context.SaveChangesAsync();

                return true; // Actualización exitosa

            }
        }

        public async Task<List<ProductoPrivadoDTO>> GetProductosKitById(int id)
        {
            var productoKit = await _context.KitsProductos.Where(kp => kp.KitId == id).ToListAsync();

            var productos = new List<ProductoPrivadoDTO>();

            if (productoKit != null && productoKit.Count > 0)
            {
                Console.WriteLine("Productos encontrados en el kit: " + productoKit.Count);

            }
            else
            {
                Console.WriteLine("No se encontraron productos en el kit con ID: " + id);
            }

            foreach (var kp in productoKit)
            {
                var producto = await _context.Productos.Where(p => p.Id == kp.ProductoId).FirstOrDefaultAsync();

                if (producto != null)
                {
                    var ppDTO = new ProductoPrivadoDTO()
                    {
                        id = producto.Id,
                        Nombre = producto.Nombre,
                        Subtitulo = producto.Subtitulo,
                        Descripcion = producto.Descripcion,
                        Imagenes = producto.Imagenes,
                        Precio = producto.Precio,
                        Stock = producto.Stock,
                        tipoProd = producto.tipoProd
                    };
                    productos.Add(ppDTO);
                }
            }

            return productos;
        }

        public async Task<List<int>> GetIdProductosKit(int id)
        {
            var productoKit = await _context.KitsProductos.Where(kp => kp.KitId == id).ToListAsync();
            return productoKit.Select(kp => kp.ProductoId).ToList();
        }

        public async Task<Producto?> GetProdImg(int id, string url)
        {
            var producto = await _context.Productos.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (producto != null && producto.Imagenes?.Contains(url) == true)
            {
                return producto;
            }

            return null;
        }

        public async Task<bool> DeleteProd(int id)
        {
            var isKit = await _context.KitsProductos.Where(kp => kp.KitId == id).ToListAsync();
            var isItInKit = await _context.KitsProductos.Where(kp => kp.ProductoId == id).ToListAsync();

            if (isKit.Any() && isItInKit.Any())
            {
                var producto = await _context.Productos.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    _context.KitsProductos.RemoveRange(isItInKit);
                    _context.KitsProductos.RemoveRange(isKit);

                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            else if (isKit.Any())
            {
                var producto = await _context.Productos.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    _context.KitsProductos.RemoveRange(isKit);

                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            else if (isItInKit.Any())

            {
                var producto = await _context.Productos.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    _context.KitsProductos.RemoveRange(isItInKit);

                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            else
            {
                var producto = await _context.Productos.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (producto != null)
                {
                    _context.Productos.Remove(producto);

                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            }

        }
    }
}

