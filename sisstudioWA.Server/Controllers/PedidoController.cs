using Microsoft.AspNetCore.Mvc;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Repositorio.Repositorios;

namespace sisstudioWA.Server.Controllers
{
    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepositorio repositorio;

        public PedidoController(IPedidoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet] // api/pedido
        public async Task<ActionResult<List<Pedido>>> ListaPedidos()
        {
            var pedidos = await repositorio.Select();
            return Ok(pedidos);
        }

        [HttpGet("{id:int}")] // api/pedido/{id}
        public async Task<ActionResult<Pedido>> ObtenerPedidoPorId(int id)
        {
            var pedido = await repositorio.SelectById(id);
            if (pedido != null)
            {
                return Ok(pedido);
            }
            return NotFound();
        }

        [HttpPost] // api/pedido

        public async Task<ActionResult> CrearPedido(Pedido pedido)
        {
            await repositorio.Insert(pedido);
            return CreatedAtAction(nameof(ObtenerPedidoPorId), new { id = pedido.Id }, pedido);
        }   

        [HttpPut("{id:int}")] // api/pedido/{id}

        public async Task<ActionResult> ActualizarPedido(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }
            var pedidoExistente = await repositorio.SelectById(id);
            if (pedidoExistente == null)
            {
                return NotFound();
            }
            await repositorio.Update( id, pedido);
            return NoContent();
        }

        [HttpDelete("{id:int}")] // api/pedido/{id}

        public async Task<ActionResult> EliminarPedido(int id)
        {
            var pedidoExistente = await repositorio.SelectById(id);
            if (pedidoExistente == null)
            {
                return NotFound();
            }
            await repositorio.Delete(id);
            return NoContent();
        }

    }
}
