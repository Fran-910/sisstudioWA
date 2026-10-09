using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Repositorio.Repositorios;
using sisstudioWA.Shared.DTO;
using sisstudioWA.Shared.Enum;

namespace sisstudioWA.Server.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepositorio repositorio;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public ProductoController(IProductoRepositorio repositorio ,IWebHostEnvironment env, IConfiguration config)
        {
            this.repositorio = repositorio;
            this.environment = env;
            this.configuration = config;
        }


        [HttpGet] // api/producto
        public async Task<ActionResult<List<ProductoPublicoDTO>>> ListaProductosClientes()
        {

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
        public async Task<ActionResult<List<ProductoPrivadoDTO>>> ListaProductosAdmin()
        {
            var productos = await repositorio.Select();
            return Ok(productos);
        }

        [HttpGet("{id:int}")] // api/producto/{id}
        public async Task<ActionResult<Producto>> ObtenerProductoPorId(int id)
        {
            var producto = await repositorio.SelectById(id);
            if (producto != null)
            {
                return Ok(producto);
            }
            else
            {
                return NotFound("Producto no encontrado");
            }
        }

        [HttpPost("crear")] // api/producto
        public async Task<ActionResult> CrearProducto([FromForm] IFormFileCollection imagenes, [FromForm] CUProductoRequestDTO DTO)//List<int>? idProductosComponentes)
        {

            Producto producto = new Producto();

            producto.Nombre = DTO.DTO.Nombre;
            producto.Subtitulo = DTO.DTO.Subtitulo;
            producto.Descripcion = DTO.DTO.Descripcion;
            producto.Imagenes = DTO.DTO.Imagenes;
            producto.Precio = DTO.DTO.Precio;
            producto.tipoProd = DTO.DTO.tipoProd;
            producto.Stock = DTO.DTO.Stock;

            var path = configuration["Storage:ProductosPath"];
            var rutasWeb = new List<string>();
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(environment.ContentRootPath, path);
            }

            foreach (var img in imagenes)
            {
                if (img.Length > 0)
                {
                    // Aquí puedes realizar validaciones adicionales si es necesario
                    // Por ejemplo, verificar el tipo de archivo, tamaño máximo, etc.
                    var extension = Path.GetExtension(img.FileName);
                    var prodFolder = Path.Combine(path, producto.Nombre);
                    Directory.CreateDirectory(prodFolder);
                    var nombreArchivo = $"{producto.Nombre}_{Guid.NewGuid().ToString()}{extension}";
                    var rutaArchivo = Path.Combine(prodFolder, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var rutaWeb = $"/uploads/{producto.Nombre}/{nombreArchivo}";
                    rutasWeb.Add(rutaWeb);
                }
                else
                {
                    return BadRequest("Uno o más archivos de imagen están vacíos.");
                }
            }

            if(rutasWeb.Count > 0)
            {
                producto.Imagenes = rutasWeb.ToArray();
            }
            else
            {
                return BadRequest("No se proporcionaron imágenes válidas para el producto.");
            }

            if (producto.tipoProd == TipoProd.Producto)
            {
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
            else if (producto.tipoProd == TipoProd.Kit)
            {

                foreach (var id in DTO.idProductosKit ?? new List<int>())
                {
                    if (id <= 0)
                    {
                        return BadRequest("Los IDs de los productos componentes deben ser mayores que cero.");
                    }

                    Console.WriteLine("ID del producto componente: " + id);
                }

                if (DTO.idProductosKit == null || DTO.idProductosKit.Count() == 0)
                {
                    return BadRequest("Debe proporcionar al menos un ID de producto componente para crear un Kit.");
                }
                else
                {
                    bool resultado = await repositorio.InsertKit(producto, DTO.idProductosKit);
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
            return BadRequest("Tipo de producto no válido");
        }

        [HttpPut("{id:int}")] // api/producto/{id}
        public async Task<ActionResult<bool>> ActualizarProducto(ProductoPrivadoDTO DTO)
        {
            Producto productoActualizado = new Producto();
            productoActualizado.Nombre = DTO.Nombre;
            productoActualizado.Subtitulo = DTO.Subtitulo;
            productoActualizado.Descripcion = DTO.Descripcion;
            productoActualizado.Imagenes = DTO.Imagenes;
            productoActualizado.Precio = DTO.Precio;
            productoActualizado.tipoProd = DTO.tipoProd;
            productoActualizado.Stock = DTO.Stock;

            bool actualizado = await repositorio.Update(DTO.id, productoActualizado);

            if (actualizado)
            {
                return Ok(actualizado);
            }
            return BadRequest("Error al actualizar el producto: " + actualizado);
        }

        [HttpPut("kit/{id:int}")] // api/producto/kit/{id}

        public async Task<ActionResult<bool>> ActualizarKit(CUProductoRequestDTO DTO)
        {
            Producto producto = new Producto();
            producto.Nombre = DTO.DTO.Nombre;
            producto.Subtitulo = DTO.DTO.Subtitulo;
            producto.Descripcion = DTO.DTO.Descripcion;
            producto.Imagenes = DTO.DTO.Imagenes;
            producto.Precio = DTO.DTO.Precio;
            producto.tipoProd = DTO.DTO.tipoProd;
            producto.Stock = DTO.DTO.Stock;

            if (DTO.DTO.id <= 0)
            {
                return BadRequest("El ID del producto es inválido.");
            }
            if(DTO.idProductosKit == null || DTO.idProductosKit.Count == 0 || DTO.idProductosKit.Any(x => x <= 0))
            {
                return BadRequest("Debe proporcionar al menos un ID de producto componente para actualizar un Kit.");
            }

            bool actualizado = await repositorio.UpdateKit(DTO.DTO.id, producto, DTO.idProductosKit);

            if (actualizado)
            {
                return Ok(actualizado);
            }
            else
            {
                return BadRequest("Error al actualizar el kit: " + actualizado);
            }


        }

        [HttpDelete("{id:int}")] // api/producto/{id}
        public async Task<ActionResult<bool>> DeleteProducto(int id)
        {
            bool eliminado = await repositorio.Delete(id);
            return Ok(eliminado);
        }

        [HttpGet("kit/{id:int}")] // api/producto/kit/{id}
        public async Task<ActionResult<List<Producto>>> GetProductosKitByIdAdmin(int id)
        {
            var productosKit = await repositorio.GetProductosKitById(id);
            if (productosKit != null && productosKit.Count > 0)
            {
                return Ok(productosKit);
            }
            else
            {
                return NotFound("No se encontraron productos para el kit con el ID proporcionado");
            }
        }
    }
}
