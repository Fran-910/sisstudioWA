using Microsoft.AspNetCore.Mvc;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Repositorio.Repositorios;

namespace sisstudioWA.Server.Controllers
{
    [ApiController]
    [Route("api/carrito")]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoRepositorio repositorio;

        public CarritoController(ICarritoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet] // api/carrito
        public async Task<ActionResult<List<Carrito>>> ListaCarritos()
        {
            var carritos = await repositorio.Select();
            return Ok(carritos);
        }

        [HttpGet("{id:int}")] // api/carrito/{id}
        public async Task<ActionResult<Carrito>> ObtenerCarritoPorId(int id)
        {
            var carrito = await repositorio.SelectById(id);
            if (carrito != null)
            {
                return Ok(carrito);
            }
            return NotFound();
        }

        [HttpPost] // api/carrito

        public async Task<ActionResult> CrearCarrito(Carrito carrito)
        {
            await repositorio.Insert(carrito);
            return CreatedAtAction(nameof(ObtenerCarritoPorId), new { id = carrito.Id }, carrito);
        }

        [HttpPut("{id:int}")] // api/carrito/{id}

        public async Task<ActionResult> ActualizarCarrito(int id, Carrito carrito)
        {
            if (id != carrito.Id)
            {
                return BadRequest();
            }
            var carritoExistente = await repositorio.SelectById(id);
            if (carritoExistente == null)
            {
                return NotFound();
            }
            await repositorio.Update(id, carrito);
            return NoContent();
        }

        [HttpDelete("{id:int}")] // api/carrito/{id}

        public async Task<ActionResult> EliminarCarrito(int id)
        {
            var carritoExistente = await repositorio.SelectById(id);
            if (carritoExistente == null)
            {
                return NotFound();
            }
            await repositorio.Delete(id);
            return NoContent();
        }

    }
}
