using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Repositorio.Repositorios;
using sisstudioWA.Shared.DTO;
using sisstudioWA.Shared.Enum;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace sisstudioWA.Server.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepositorio repositorio;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public ProductoController(IProductoRepositorio repositorio, IWebHostEnvironment env, IConfiguration config)
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
        public async Task<ActionResult> CrearProducto([FromForm] IFormFileCollection imagenes, [FromForm] string producto)//List<int>? idProductosComponentes)
        {
            if (string.IsNullOrEmpty(producto))
            {
                return BadRequest("El parámetro 'producto' no puede estar vacío.");
            }
            var DTO = JsonSerializer.Deserialize<CUProductoRequestDTO>(producto, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            if (DTO?.DTO == null) return BadRequest("Datos del producto inválidos.");


            Producto prod = new Producto();

            prod.Nombre = DTO.DTO.Nombre;
            prod.Subtitulo = DTO.DTO.Subtitulo;
            prod.Descripcion = DTO.DTO.Descripcion;
            prod.Precio = DTO.DTO.Precio;
            prod.tipoProd = DTO.DTO.tipoProd;
            prod.Stock = DTO.DTO.Stock;

            var path = GetStorageRoot();
            var rutasWeb = new List<string>();


            if (DTO.DTO.tipoProd == TipoProd.Producto)
            {
                bool resultado = await repositorio.Insert(prod);
                if (resultado)
                {
                    foreach (var img in imagenes)
                    {
                        if (img.Length > 0)
                        {
                            var extension = Path.GetExtension(img.FileName);
                            var prodFolder = Path.Combine(path, prod.Id.ToString());
                            Directory.CreateDirectory(prodFolder);
                            var nombreArchivo = $"{DTO.DTO.Nombre}_{Guid.NewGuid().ToString()}{extension}";
                            var rutaArchivo = Path.Combine(prodFolder, nombreArchivo);

                            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                            {
                                await img.CopyToAsync(stream);
                            }

                            var rutaWeb = $"/uploads/{prod.Id}/{nombreArchivo}";
                            rutasWeb.Add(rutaWeb);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    prod.Imagenes = rutasWeb;

                    bool addImgs = await repositorio.Update(prod.Id, prod);
                    if (addImgs)
                    {
                        return Ok("Producto creado exitosamente");
                    }

                    return BadRequest("Error al crear el producto");
                }
                else
                {
                    return BadRequest("Error al crear el producto, no res");
                }
            }
            else if (DTO.DTO.tipoProd == TipoProd.Kit)
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
                    bool resultado = await repositorio.InsertKit(prod, DTO.idProductosKit);
                    if (resultado)
                    {
                        foreach (var img in imagenes)
                        {
                            if (img.Length > 0)
                            {
                                var extension = Path.GetExtension(img.FileName);
                                var prodFolder = Path.Combine(path, prod.Id.ToString());
                                Directory.CreateDirectory(prodFolder);
                                var nombreArchivo = $"{DTO.DTO.Nombre}_{Guid.NewGuid().ToString()}{extension}";
                                var rutaArchivo = Path.Combine(prodFolder, nombreArchivo);

                                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                                {
                                    await img.CopyToAsync(stream);
                                }

                                var rutaWeb = $"/uploads/{prod.Id}/{nombreArchivo}";
                                rutasWeb.Add(rutaWeb);
                            }
                                else
                                {
                                    continue;
                                }
                            }

                            prod.Imagenes = rutasWeb;


                        bool addImgs = await repositorio.Update(prod.Id, prod);
                        if (addImgs)
                        {
                            return Ok("Producto creado exitosamente");
                        }

                        return BadRequest("Error al crear el producto, kit");
                    }
                    else
                    {
                        return BadRequest("Error al crear el producto, reskit");
                    }
                }
            }
            return BadRequest("Tipo de producto no válido");
        }

        [HttpPut("{id:int}")] // api/producto/{id}
        public async Task<ActionResult<bool>> ActualizarProducto([FromForm] IFormFileCollection imagenes, [FromForm] string producto)
        {
            if (string.IsNullOrEmpty(producto))
            {
                return BadRequest("El parámetro 'producto' no puede estar vacío.");
            }
            var DTO = JsonSerializer.Deserialize<CUProductoRequestDTO>(producto, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            if (DTO?.DTO == null) return BadRequest("Datos del producto inválidos.");

            Producto productoActualizado = new Producto();
            productoActualizado.Nombre = DTO.DTO.Nombre;
            productoActualizado.Subtitulo = DTO.DTO.Subtitulo;
            productoActualizado.Descripcion = DTO.DTO.Descripcion;
            productoActualizado.Imagenes = DTO.DTO.Imagenes;
            productoActualizado.Precio = DTO.DTO.Precio;
            productoActualizado.tipoProd = DTO.DTO.tipoProd;
            productoActualizado.Stock = DTO.DTO.Stock;

            var path = GetStorageRoot();

            var rutasWeb = new List<string>();
            rutasWeb.AddRange(DTO.DTO.Imagenes ?? new List<string>());

            foreach (var img in imagenes)
            {
                if (img.Length > 0)
                {
                    var extension = Path.GetExtension(img.FileName);
                    var prodFolder = Path.Combine(path, DTO.DTO.id.ToString());
                    Directory.CreateDirectory(prodFolder);
                    var nombreArchivo = $"{DTO.DTO.Nombre}_{Guid.NewGuid().ToString()}{extension}";
                    var rutaArchivo = Path.Combine(prodFolder, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var rutaWeb = $"/uploads/{DTO.DTO.id}/{nombreArchivo}";
                    rutasWeb.Add(rutaWeb);
                }
                else
                {
                    return BadRequest("Uno o más archivos de imagen están vacíos.");
                }
            }

            productoActualizado.Imagenes = rutasWeb;


            bool actualizado = await repositorio.Update(DTO.DTO.id, productoActualizado);

            if (actualizado)
            {
                return Ok(actualizado);
            }
            return BadRequest("Error al actualizar el producto: " + actualizado);
        }

        [HttpPut("kit/{id:int}")] // api/producto/kit/{id}

        public async Task<ActionResult<bool>> ActualizarKit([FromForm] IFormFileCollection imagenes, [FromForm] string producto)
        {

            if (string.IsNullOrEmpty(producto))
            {
                return BadRequest("El parámetro 'producto' no puede estar vacío.");
            }
            var DTO = JsonSerializer.Deserialize<CUProductoRequestDTO>(producto, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            if (DTO?.DTO == null) return BadRequest("Datos del producto inválidos.");

            Producto productoActualizado = new Producto();
            productoActualizado.Nombre = DTO.DTO.Nombre;
            productoActualizado.Subtitulo = DTO.DTO.Subtitulo;
            productoActualizado.Descripcion = DTO.DTO.Descripcion;
            productoActualizado.Imagenes = DTO.DTO.Imagenes;
            productoActualizado.Precio = DTO.DTO.Precio;
            productoActualizado.tipoProd = DTO.DTO.tipoProd;
            productoActualizado.Stock = DTO.DTO.Stock;

            var path = GetStorageRoot();
            var rutasWeb = new List<string>();
            rutasWeb.AddRange(DTO.DTO.Imagenes ?? new List<string>());

            foreach (var img in imagenes)
            {
                if (img.Length > 0)
                {
                    var extension = Path.GetExtension(img.FileName);
                    var prodFolder = Path.Combine(path, DTO.DTO.id.ToString());
                    Directory.CreateDirectory(prodFolder);
                    var nombreArchivo = $"{DTO.DTO.Nombre}_{Guid.NewGuid().ToString()}{extension}";
                    var rutaArchivo = Path.Combine(prodFolder, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var rutaWeb = $"/uploads/{DTO.DTO.id}/{nombreArchivo}";
                    rutasWeb.Add(rutaWeb);
                }
                else
                {
                    return BadRequest("Uno o más archivos de imagen están vacíos.");
                }
            }

            productoActualizado.Imagenes = rutasWeb;

            if (DTO.DTO.id <= 0)
            {
                return BadRequest("El ID del producto es inválido.");
            }
            if (DTO.idProductosKit == null || DTO.idProductosKit.Count == 0 || DTO.idProductosKit.Any(x => x <= 0))
            {
                return BadRequest("Debe proporcionar al menos un ID de producto componente para actualizar un Kit.");
            }

            bool actualizado = await repositorio.UpdateKit(DTO.DTO.id, productoActualizado, DTO.idProductosKit);

            if (actualizado)
            {
                return Ok(actualizado);
            }
            else
            {
                return BadRequest("Error al actualizar el kit: " + actualizado);
            }
        }

        [HttpDelete("borrar/{id:int}")] // api/producto/borrar/{id}
        public async Task<ActionResult<bool>> DeleteProducto(int id)
        {
            bool eliminado = await repositorio.DeleteProd(id);
            var storagePath = GetStorageRoot();
            var productFolder = Path.Combine(storagePath, id.ToString());
            if (eliminado)
            {
                if (Directory.Exists(productFolder))
                {
                    Directory.Delete(productFolder, true);
                }
                return Ok(eliminado);
            }
            return BadRequest(eliminado);
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

        [HttpDelete("delimg")]

        public async Task<ActionResult<bool>> DeleteImagen([FromBody] ImageDelDTO imgDelDto)
        {

            var path = GetStorageRoot();

            if (!(imgDelDto != null && imgDelDto.id > 0 && imgDelDto.url != null))
            {
                return BadRequest(false);
            }

            var prod = await repositorio.SelectById(imgDelDto.id);

            foreach (var img in imgDelDto.url)
            {
                var prodImgExist = await repositorio.GetProdImg(prod.Id, img);

                if (prodImgExist != null)
                {
                    var productoPath = Path.Combine(path, prod.Id.ToString());
                    var nombreArchivo = Path.GetFileName(img);
                    var rutaFisicaCompleta = Path.Combine(productoPath, nombreArchivo);
                    if (Directory.Exists(productoPath))
                    {
                        if (System.IO.File.Exists(rutaFisicaCompleta))
                        {
                            System.IO.File.Delete(rutaFisicaCompleta);
                        }
                    }
                    prod.Imagenes.Remove(img);
                }
                else
                {
                    return BadRequest(false);
                }
            }

            await repositorio.Update(prod.Id, prod);
            return Ok(true);
        }

        private string GetStorageRoot()
        {
            var path = configuration["Storage:ProductosPath"];
            return Path.IsPathRooted(path) ? path : Path.Combine(environment.ContentRootPath, path);
        }
    }
}
