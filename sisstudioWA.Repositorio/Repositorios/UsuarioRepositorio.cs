using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Repositorio.Repositorios
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly AppDbContext context;

        public UsuarioRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
