using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Repositorio.Repositorios;
using sisstudioWA.Shared.DTO;

namespace sisstudioWA.Server.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoController : ControllerBase
    {
        private readonly Repositorio<Producto> repositorio;

        #region OldContextDb
        //private readonly AppDbContext context;

        //public ProductoController(AppDbContext context)
        //{
        //    this.context = context;
        //}
        #endregion

        public ProductoController(Repositorio<Producto> repositorio)
        {
            this.repositorio = repositorio;
        }


        [HttpGet] // api/producto
        public async Task<ActionResult<List<ProductoPublicoDTO>>> ListaDeProductosClientes()
        {
            #region OldController
            //var productos = await context.Productos.ToListAsync();
            //if (productos != null)
            //{
            //    return Ok(productos
            //        .Select(p => new ProductoPublicoDTO
            //        {
            //            Nombre = p.Nombre,
            //            Subtitulo = p.Subtitulo,
            //            Descripcion = p.Descripcion,
            //            Precio = p.Precio,
            //            HayStock = (p.Stock > 0),
            //            tipoProd = p.tipoProd
            //        }).ToList());
            //}
            //else
            //{
            //    return NotFound("No se encontraron productos");
            //}
            #endregion

            var productos = await repositorio.Select();

            List<ProductoPublicoDTO> productoPublicoDTOs = new List<ProductoPublicoDTO>();

            foreach (var product in productos)
            {
                ProductoPublicoDTO publico = new ProductoPublicoDTO();
                publico.Nombre = product.Nombre;
                publico.Subtitulo = product.Subtitulo;
                publico.Descripcion = product.Descripcion;
                publico.Precio = product.Precio;
                publico.HayStock = (product.Stock > 0);
                publico.tipoProd = product.tipoProd;
                productoPublicoDTOs.Add(publico);
            }

            return Ok(productoPublicoDTOs);
        }

        [HttpGet("admin")]
        public async Task<ActionResult<List<Producto>>> ListaProductosAdmin()
        {
            var productos = await repositorio.Select();
            return Ok(productos);
        }

        [HttpPost] // api/producto
        public async Task<ActionResult> CrearProducto(Producto nuevoProducto)
        {
            Producto producto = new Producto();
            producto.Nombre = nuevoProducto.Nombre;
            producto.Subtitulo = nuevoProducto.Subtitulo;
            producto.Descripcion = nuevoProducto.Descripcion;
            producto.Precio = nuevoProducto.Precio;
            producto.tipoProd = nuevoProducto.tipoProd;
            producto.Stock = nuevoProducto.Stock;

            bool resultado = await repositorio.Insert(producto);
            if (resultado)
            {
                return Ok("Producto creado exitosamente");
            }
            else
            {
                return BadRequest("Error al crear el producto");
            }
        }
    }
}
