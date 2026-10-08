using Microsoft.AspNetCore.Mvc;
using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using sisstudioWA.Shared.DTO;
using sisstudioWA.Shared.Enum;

namespace sisstudioWA.Server.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio repositorio; 
        public UsuarioController(IUsuarioRepositorio repositorio)
        {
            this.repositorio = repositorio; 
        }

        [HttpGet] // api/usuario
        public async Task<ActionResult<List<Usuario>>> ListaUsuarios()
        {
            var usuarios = await repositorio.Select();
            return Ok(usuarios);
        }

        [HttpGet("{id:int}")] // api/usuario/{id}
        public async Task<ActionResult<Usuario>> ObtenerUsuarioPorId(int id)
        {
            var usuario = await repositorio.SelectById(id);
            if (usuario != null)
            {
                return Ok(usuario);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost] // api/usuario
        public async Task<ActionResult> CrearUsuario(Usuario usuario)
        {
            await repositorio.Insert(usuario);
            return Ok();
        }

        [HttpPut("{id:int}")] // api/usuario/{id}
        public async Task<ActionResult> ActualizarUsuario(int id, Usuario usuario)
        {
            var usuarioExistente = await repositorio.SelectById(id);
            if (usuarioExistente != null)
            {
                usuario.Id = id;
                await repositorio.Update(id, usuario);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")] // api/usuario/{id}
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            var usuarioExistente = await repositorio.SelectById(id);
            if (usuarioExistente != null)
            {
                await repositorio.Delete(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
