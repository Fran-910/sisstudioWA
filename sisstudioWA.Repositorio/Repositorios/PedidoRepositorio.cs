using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Repositorio.Repositorios
{
    public class PedidoRepositorio : Repositorio<Pedido>, IPedidoRepositorio
    {
        private readonly AppDbContext context;

        public PedidoRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
